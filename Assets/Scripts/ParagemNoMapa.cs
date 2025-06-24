using UnityEngine;
using System.Collections.Generic;

public class ParagemNoMapa : MonoBehaviour
{

    public int numeroDestinoHUD;
    public int idParagem;

    [Tooltip("Transform usado como ponto de origem dos NPCs")]
    public Transform spawnOrigin;
    public BusStop busStop;

    public List<GameObject> objetosSpawnados = new List<GameObject>();
    public Vector3 direcaoDistribuicao = Vector3.right;

    public GameObject paragemHUD;


    public void SpawnarObjeto(GameObject prefab)
    {
        if (prefab == null || spawnOrigin == null)
        {
            Debug.LogWarning($"⚠️ Prefab ou spawnOrigin não atribuído na paragem {idParagem}.");
            return;
        }

        int index = objetosSpawnados.Count;
        Vector3 offset = direcaoDistribuicao.normalized * index * 2f;

        GameObject novo = Instantiate(prefab, spawnOrigin);
        novo.transform.localPosition = offset;

        Person personComp = novo.GetComponent<Person>();
        if (personComp != null)
        {
            personComp.idParagemOrigem = idParagem;
            personComp.paragemHUD = paragemHUD;

            Debug.Log($"👤 NPC criado com sucesso na paragem {idParagem}");
        }
        else
        {
            Debug.LogWarning($"⚠️ Objeto criado na paragem {idParagem}, mas não tem componente 'Person'");
        }

        objetosSpawnados.Add(novo);
    }
}