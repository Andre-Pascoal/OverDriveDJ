using UnityEngine;
using UnityEngine.AI;

public class AiSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] aiPrefabs; // Agora um array de prefabs
    [SerializeField] private int numberToSpawn = 20;
    [SerializeField] private Vector3 mapSize = new Vector3(100f, 0, 100f);
    [SerializeField] private int maxAttemptsPerAI = 20;
    [SerializeField] private float sampleMaxDistance = 2f;

    void Start()
    {
        for (int i = 0; i < numberToSpawn; i++)
        {
            TrySpawnAI();
        }
    }

    void TrySpawnAI()
    {
        int attempts = 0;

        int walkableArea = NavMesh.GetAreaFromName("Walkable");
        int areaMask = 1 << walkableArea;

        while (attempts < maxAttemptsPerAI)
        {
            Vector3 randomPoint = GetRandomPointInMap();
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, sampleMaxDistance, areaMask))
            {
                GameObject prefabToSpawn = GetRandomPrefab();
                GameObject obj = Instantiate(prefabToSpawn, hit.position, Quaternion.identity);

                NavMeshAgent agent = obj.GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    agent.Warp(hit.position);
                }

                return;
            }

            attempts++;
        }

        Debug.LogWarning("Não foi possível encontrar ponto válido na área WALKABLE.");
    }

    Vector3 GetRandomPointInMap()
    {
        float x = Random.Range(-mapSize.x / 2f, mapSize.x / 2f);
        float z = Random.Range(-mapSize.z / 2f, mapSize.z / 2f);
        return new Vector3(x, 0, z);
    }

    GameObject GetRandomPrefab()
    {
        if (aiPrefabs.Length == 0)
        {
            Debug.LogError("Nenhum prefab definido no AiSpawner.");
            return null;
        }

        int index = Random.Range(0, aiPrefabs.Length);
        return aiPrefabs[index];
    }
}
