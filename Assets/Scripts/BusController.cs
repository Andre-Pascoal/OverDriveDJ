using System.Collections.Generic;
using UnityEngine;

public class BusController : MonoBehaviour
{

    [Header("Prefab para pessoa ao sair")]
    public GameObject personPrefabSaida;
    public List<Person> passageiros = new List<Person>();

    public void AdicionarPassageiro(Person person)
    {
        person.alturaOriginalY = person.transform.position.y;
        passageiros.Add(person);
        person.tempoEntrada = Time.time;
        person.gameObject.SetActive(false);

        // Altera o número da paragem HUD para o destino
        if (person.paragemHUD != null)
        {
            var mascara = person.paragemHUD.GetComponent<ParagemComMascara>();
            if (mascara != null)
            {
                mascara.DefinirNumero(person.destino); // <- Atualiza o número visível no HUD
            }
        }

        Debug.Log("Pessoa entrou no autocarro. Destino: " + person.destino);
    }

    public void VerificarSaida(int idParagemAtual)
    {
        List<Person> personsASair = passageiros.FindAll(p => p.destino == idParagemAtual);

        float spacing = 1.5f;
        float lateralOffset = 5f;
        float alturaInicial = 8f;

        int i = 0;

        // Layer da estrada
        int estradaLayer = LayerMask.NameToLayer("estrada");

        foreach (Person p in personsASair)
        {
            Vector3 basePosition = transform.position + transform.right * lateralOffset;
            Vector3 offsetZ = transform.forward * (i * spacing);
            Vector3 spawnAcima = basePosition + offsetZ + Vector3.up * alturaInicial;
            Vector3 spawnFinal;

            if (Physics.Raycast(spawnAcima, Vector3.down, out RaycastHit hit, alturaInicial + 5f))
            {
                if (hit.collider.gameObject.layer == estradaLayer)
                {
                    spawnFinal = hit.point;
                    spawnFinal.y = p.alturaOriginalY;
                }
                else
                {
                    spawnFinal = basePosition + offsetZ;
                    spawnFinal.y = p.alturaOriginalY;
                }
            }
            else
            {
                spawnFinal = basePosition + offsetZ;
                spawnFinal.y = p.alturaOriginalY;
            }

            // ✅ Spawn do novo prefab ao sair
            if (personPrefabSaida != null)
            {
                Instantiate(personPrefabSaida, spawnFinal, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("Prefab de saída não atribuído!");
            }

            // ✅ Adicionar dinheiro com base na satisfação
            if (p.paragemHUD != null)
            {
                ParagemComMascara mascara = p.paragemHUD.GetComponent<ParagemComMascara>();
                if (mascara != null)
                {
                    GameManager.Instance.AdicionarDinheiro(mascara.satisfacao);
                }

                Destroy(p.paragemHUD);
            }

            // ✅ Destruir o objeto original
            Destroy(p.gameObject);

            i++;
        }

        passageiros.RemoveAll(p => p.destino == idParagemAtual);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("person"))
        {
            GameManager.Instance.dinheiroAtual -= 100;
            Debug.Log("💥 Colisão com pessoa! -100€");

            // Atualiza a UI após a perda de dinheiro
            GameManager.Instance.SendMessage("AtualizarUI", SendMessageOptions.DontRequireReceiver);
        }
    }

}
