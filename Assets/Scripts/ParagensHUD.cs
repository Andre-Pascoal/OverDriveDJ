using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;


public class ParagensHUD : MonoBehaviour
{
    [Header("Configurações de Paragens")]
    public int totalPessoas = 10;
    public Transform paragensContainer;

    [Header("Prefabs")]
    public GameObject prefabParagemBloco;  // Um único prefab para todas as paragens
    public GameObject prefabPessoaUI;      // Um único prefab para todas as pessoas

    [Header("Sprite")]
    public Sprite spriteParagemUnico;

    private const int minParagens = 1;
    private const int maxParagens = 3;
    private const int maxPessoasPorParagem = 3;

    [Header("Configuração de Números Únicos")]
    public int numeroMin = 1;
    public int numeroMax = 100;

    [Header("Prefab a ser instanciado nas paragens do mapa")]
    public GameObject prefabExtra;

    private ParagemNoMapa[] paragensNoMapa; // Cache das paragens da cena

    [Header("Waypoint Prefab")]
    public GameObject prefabWaypoint;

    public static ParagensHUD Instance;

    void Awake()
    {
        Instance = this;
    }



    void Start()
    {
        paragensNoMapa = FindObjectsOfType<ParagemNoMapa>();
        MostrarParagens();

        StartCoroutine(AdicionarParagensPeriodicamente());

    }

    void MostrarParagens()
    {
        // Limpa container
        //foreach (Transform child in paragensContainer)
        //   Destroy(child.gameObject);

        if (prefabParagemBloco == null)
        {
            Debug.LogError("Prefab de paragem não atribuído!");
            return;
        }

        if (prefabPessoaUI == null)
        {
            Debug.LogError("Prefab de pessoa não atribuído!");
            return;
        }

        int numParagens = Random.Range(minParagens, maxParagens + 1);
        Debug.Log($"Número de paragens: {numParagens}");

        int minPessoasNecessarias = numParagens;
        int maxPessoasPossiveis = numParagens * maxPessoasPorParagem;

        int pessoasDistribuir = Mathf.Clamp(totalPessoas, minPessoasNecessarias, maxPessoasPossiveis);

        List<int> pessoasPorParagem = DistribuirSemZeros(pessoasDistribuir, numParagens);
        Debug.Log("Distribuição: " + string.Join(", ", pessoasPorParagem));

        List<int> numerosUnicos = GerarNumerosUnicos(numParagens, numeroMin, numeroMax);

        for (int i = 0; i < numParagens; i++)
        {
            GameObject paragemGO = Instantiate(prefabParagemBloco, paragensContainer);
            paragemGO.name = $"Paragem_{i}";

            // Aplica o sprite
            Image imgParagem = paragemGO.GetComponent<Image>();
            if (imgParagem != null && spriteParagemUnico != null)
            {
                imgParagem.sprite = spriteParagemUnico;
                imgParagem.preserveAspect = true;
            }

            // Passa o número único para o prefab
            int numeroUnico = numerosUnicos[i];
            ParagemComMascara scriptParagem = paragemGO.GetComponent<ParagemComMascara>();
            if (scriptParagem != null)
            {
                scriptParagem.DefinirNumero(numeroUnico);
            }

            // Número de pessoas nesta paragem
            int numPessoas = pessoasPorParagem[i];


            // Spawn de objetos extras no mapa
            foreach (ParagemNoMapa paragemMapa in paragensNoMapa)
            {
                if (paragemMapa.idParagem == numeroUnico)
                {
                    paragemMapa.paragemHUD = paragemGO; // <--- adiciona esta linha
                    int destino = EscolherDestinoAleatorio(paragemMapa.idParagem);
                    for (int j = 0; j < numPessoas; j++)
                    {
                        paragemMapa.SpawnarObjeto(prefabExtra);
                    }
                    break;
                }
            }


            // Adiciona pessoas no HUD
            Transform containerPessoas = paragemGO.transform.Find("PessoasContainer");
            if (containerPessoas == null)
            {
                Debug.LogWarning("Prefab de paragem precisa ter um filho chamado 'PessoasContainer'");
                continue;
            }

            for (int j = 0; j < numPessoas; j++)
            {
                GameObject pessoaGO = Instantiate(prefabPessoaUI, containerPessoas);
                pessoaGO.name = $"Pessoa_{i}_{j}";
            }
        }   
    }

    List<int> DistribuirSemZeros(int total, int n)
    {
        List<int> distrib = new List<int>(new int[n]);

        for (int i = 0; i < n; i++)
        {
            distrib[i] = 1;
            total--;
        }

        while (total > 0)
        {
            List<int> candidatos = new List<int>();
            for (int i = 0; i < n; i++)
                if (distrib[i] < maxPessoasPorParagem)
                    candidatos.Add(i);

            if (candidatos.Count == 0)
                break;

            int idx = candidatos[Random.Range(0, candidatos.Count)];
            distrib[idx]++;
            total--;
        }

        return distrib;
    }

    List<int> GerarNumerosUnicos(int quantidade, int min, int max)
    {
        List<int> todos = new List<int>();
        for (int i = min; i <= max; i++)
            todos.Add(i);

        if (quantidade > todos.Count)
        {
            Debug.LogError("Intervalo de números insuficiente para gerar valores únicos.");
            return new List<int>();
        }

        for (int i = todos.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int tmp = todos[i];
            todos[i] = todos[j];
            todos[j] = tmp;
        }

        return todos.GetRange(0, quantidade);
    }

    int EscolherDestinoAleatorio(int idParagemAtual)
    {

        List<int> idsParagens = new List<int>();

        foreach (ParagemNoMapa p in paragensNoMapa)
        {
            if (p.idParagem != idParagemAtual)
                idsParagens.Add(p.idParagem);
        }

        if (idsParagens.Count == 0)
        {
            Debug.LogWarning("Não há outras paragens para escolher destino.");
            return idParagemAtual;  // fallback: retorna o próprio id atual
        }

        int indice = Random.Range(0, idsParagens.Count);
        return idsParagens[indice];
    }

    IEnumerator AdicionarParagensPeriodicamente()
    {
        while (true)
        {
            yield return new WaitForSeconds(60f); // Espera 45 segundos
            MostrarParagens(); // Adiciona mais paragens e passageiros
        }
    }

}