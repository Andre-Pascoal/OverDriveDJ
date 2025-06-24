using UnityEngine;
using UnityEngine.AI;

public class AiPatrol : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform[] waypoints;
    private Transform currentWaypoint;

    [SerializeField] private string waypointTag = "Waypoint";

    private GroupMovementController currentGroupController;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        GameObject[] waypointObjects = GameObject.FindGameObjectsWithTag(waypointTag);
        if (waypointObjects.Length == 0)
        {
            Debug.LogWarning("No waypoints found with tag: " + waypointTag);
            return;
        }

        waypoints = new Transform[waypointObjects.Length];
        for (int i = 0; i < waypointObjects.Length; i++)
        {
            waypoints[i] = waypointObjects[i].transform;
        }

        currentWaypoint = GetRandomWaypoint();
        agent.SetDestination(currentWaypoint.position);
    }

    void Update()
    {
        // Se estiver dentro de uma zona com GroupMovementController e não pode mover
        if (currentGroupController != null && !currentGroupController.canMove)
        {
            agent.isStopped = true;
            return;
        }
        else
        {
            agent.isStopped = false;
        }

        if (!agent.pathPending && agent.remainingDistance < 1f)
        {
            Transform nextWaypoint = GetRandomWaypointExcluding(currentWaypoint);
            currentWaypoint = nextWaypoint;
            agent.SetDestination(currentWaypoint.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GroupMovementController controller = other.GetComponentInParent<GroupMovementController>();
        if (controller != null)
        {
            currentGroupController = controller;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        GroupMovementController controller = other.GetComponent<GroupMovementController>();
        if (controller != null && controller == currentGroupController)
        {
            currentGroupController = null;
        }
    }


    Transform GetRandomWaypoint()
    {
        return waypoints[Random.Range(0, waypoints.Length)];
    }

    Transform GetRandomWaypointExcluding(Transform exclude)
    {
        if (waypoints.Length <= 1)
            return exclude;

        Transform next;
        do
        {
            next = GetRandomWaypoint();
        } while (next == exclude);

        return next;
    }
}
