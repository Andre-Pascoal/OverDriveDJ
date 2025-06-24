using System.Collections.Generic;
using UnityEngine;

public class BusStop : MonoBehaviour
{

    public int idParagem;
    public GameObject personPrefab;

    private float tempoNecessario = 3f;
    private float tempoDentro = 0f;
    private bool autocarroPresente = false;
    private GameObject autocarro;

    private List<GameObject> pessoasNaParagem = new List<GameObject>();
    private float tempoParaProximaGeracao = 0f;
    private bool geracaoInicialFeita = false;

    private ParagemNoMapa paragemMapa;

    void Awake()
    {
        paragemMapa = GetComponent<ParagemNoMapa>();
    }


    void OnTriggerEnter(Collider other)
    {
        GameObject root = other.transform.root.gameObject;
        if (root.CompareTag("bus"))
        {
            autocarro = root;
            autocarroPresente = true;
            tempoDentro = 0f;
            Debug.Log("Bus Entrou (via filho): " + root.name);
        }
    }

    void OnTriggerStay(Collider other)
    {
        GameObject root = other.transform.root.gameObject;
        if (root.CompareTag("bus"))
        {
            tempoDentro += Time.deltaTime;

            if (tempoDentro >= tempoNecessario)
            {
                BusController bus = autocarro.GetComponent<BusController>();
                if (bus != null)
                {
                    // Passa o ID da paragem atual, não o nome
                    TransferirPessoas(bus);
                    bus.VerificarSaida(idParagem);
                }

                autocarroPresente = false;
                tempoDentro = 0f;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        GameObject root = other.transform.root.gameObject;
        if (root.CompareTag("bus"))
        {
            autocarroPresente = false;
            tempoDentro = 0f;
            Debug.Log("Bus saiu (via filho): " + root.name);
        }
    }

    void TransferirPessoas(BusController bus)
    {
        if (paragemMapa == null)
        {
            Debug.LogWarning("ParagemMapa não atribuída no BusStop.");
            return;
        }

        List<GameObject> pessoasParaRemover = new List<GameObject>();

        foreach (GameObject obj in paragemMapa.objetosSpawnados)
        {
            if (obj != null)
            {
                Person p = obj.GetComponent<Person>();
                if (p != null)
                {
                    bus.AdicionarPassageiro(p);
                    pessoasParaRemover.Add(obj);
                }
            }
        }

        // Remover do mundo
        foreach (GameObject p in pessoasParaRemover)
        {
            paragemMapa.objetosSpawnados.Remove(p);
            p.gameObject.SetActive(false);
        }

        // Atualizar número de pessoas no HUD
        if (paragemMapa.paragemHUD != null)
        {
            ParagemComMascara mascara = paragemMapa.paragemHUD.GetComponent<ParagemComMascara>();
            if (mascara != null)
            {
                // Verifica se há alguém ainda sem destino atribuído
                bool algumSemDestino = false;

                foreach (GameObject obj in pessoasParaRemover)
                {
                    Person p = obj.GetComponent<Person>();
                    if (p != null && !p.destinoAtribuido)
                    {
                        algumSemDestino = true;
                        break;
                    }
                }

                if (algumSemDestino)
                {
                    int novoNumero;

                    do
                    {
                        novoNumero = Random.Range(ParagensHUD.Instance.numeroMin, ParagensHUD.Instance.numeroMax + 1);
                    }
                    while (novoNumero == paragemMapa.idParagem);

                    // Atribuir novo destino a todos os passageiros SEM destino
                    foreach (GameObject obj in pessoasParaRemover)
                    {
                        Person p = obj.GetComponent<Person>();
                        if (p != null && !p.destinoAtribuido)
                        {
                            p.destino = novoNumero;
                            p.destinoAtribuido = true;
                        }
                    }

                    // Atualizar o texto visível no HUD
                    mascara.DefinirNumero(novoNumero);
                    paragemMapa.numeroDestinoHUD = novoNumero;
                }
            }
        }
    }
}