using UnityEngine;
using System.Collections;

public class TrafficManager : MonoBehaviour
{
    public GameObject[] carPrefabs;
    public Transform[] spawnPoints;
    public float spawnInterval = 10f;
    public int initialCarCount = 20;

    private int periodicSpawnIndex = 0;

    private void Start()
    {
        SpawnInitialCars();
        StartCoroutine(SpawnOneCarPerSpawnPoint());
    }

    void SpawnInitialCars()
    {
        for (int i = 0; i < initialCarCount; i++)
        {
            int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
            int randomCarIndex = Random.Range(0, carPrefabs.Length);

            Transform spawnPoint = spawnPoints[randomSpawnIndex];
            GameObject car = Instantiate(carPrefabs[randomCarIndex], spawnPoint.position, spawnPoint.rotation);

            CarAI carAI = car.GetComponent<CarAI>();
            if (carAI != null)
            {
                carAI.waypoints = spawnPoints;
            }
        }
    }

    IEnumerator SpawnOneCarPerSpawnPoint()
    {
        while (true)
        {
            if (spawnPoints.Length == 0 || carPrefabs.Length == 0)
                yield break;

            Transform spawnPoint = spawnPoints[periodicSpawnIndex];
            int randomCarIndex = Random.Range(0, carPrefabs.Length);

            GameObject car = Instantiate(carPrefabs[randomCarIndex], spawnPoint.position, spawnPoint.rotation);

            CarAI carAI = car.GetComponent<CarAI>();
            if (carAI != null)
            {
                carAI.waypoints = spawnPoints;
            }

            // Avança para o próximo spawn point
            periodicSpawnIndex = (periodicSpawnIndex + 1) % spawnPoints.Length;

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}