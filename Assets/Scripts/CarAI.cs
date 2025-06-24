using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class CarAI : MonoBehaviour
{
    public float minSpeed = 5f;
    public float maxSpeed = 20f;

    public Transform[] waypoints;
    private int currentIndex;
    private NavMeshAgent agent;
    private float normalSpeed;

    // Cone de visão (em graus)
    public float maxConeAngle = 30f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = Random.Range(minSpeed, maxSpeed);
        normalSpeed = agent.speed;
        PickNewDestination();
    }

    void Update()
    {
        //DetectCarAhead(); // Se quiseres ativar deteção de carros à frente

        if (!agent.pathPending && agent.remainingDistance < 1f)
        {
            PickNewDestination();
        }
    }

    void PickNewDestination()
    {
        Vector3 currentPosition = transform.position;
        Vector3 forward = transform.forward;

        List<int> validWaypoints = new List<int>();

        for (int i = 0; i < waypoints.Length; i++)
        {
            if (i == currentIndex) continue;

            Vector3 directionToWaypoint = (waypoints[i].position - currentPosition).normalized;
            float angle = Vector3.Angle(forward, directionToWaypoint);

            if (angle <= maxConeAngle)
            {
                validWaypoints.Add(i);
            }
        }

        // Fallback se não houver nenhum destino dentro do cone
        if (validWaypoints.Count == 0)
        {
            for (int i = 0; i < waypoints.Length; i++)
            {
                if (i != currentIndex)
                    validWaypoints.Add(i);
            }
        }

        int newIndex = validWaypoints[Random.Range(0, validWaypoints.Count)];
        currentIndex = newIndex;

        agent.SetDestination(waypoints[currentIndex].position);
    }

    // (Opcional) Visualização do cone no Editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 forward = transform.forward;
        float coneLength = 10f;

        Quaternion leftRayRotation = Quaternion.Euler(0, -maxConeAngle, 0);
        Quaternion rightRayRotation = Quaternion.Euler(0, maxConeAngle, 0);

        Vector3 leftRayDirection = leftRayRotation * forward;
        Vector3 rightRayDirection = rightRayRotation * forward;

        Gizmos.DrawRay(transform.position, leftRayDirection * coneLength);
        Gizmos.DrawRay(transform.position, rightRayDirection * coneLength);
        Gizmos.DrawRay(transform.position, forward * coneLength);
    }
}
