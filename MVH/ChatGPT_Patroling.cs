using System.Diagnostics;

uusing System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Patrolling : MonoBehaviour
{
    public Transform[] waypoints;
    private int currentWaypoint = 0;
    public float followDistance = 10f;
    public float returnDistance = 15f;
    public NavMeshAgent agent;
    public LayerMask playerLayer;
    private GameObject player;
    public float waypointArrivalDistance = 1f;

    void Start()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        if (waypoints.Length == 0)
        {
            Debug.LogError("");
            return;
        }

        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("");
        }
        else
        {
            StartCoroutine(Patrol());
        }
    }

    IEnumerator Patrol()
    {
        while (true)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            float distanceToWaypoint = Vector3.Distance(transform.position, waypoints[currentWaypoint].position);

            if (distanceToPlayer <= followDistance)
            {
                agent.SetDestination(player.transform.position);
            }
            else if (distanceToPlayer >= returnDistance || distanceToWaypoint <= waypointArrivalDistance)
            {
                SetNextWaypoint();
            }

            yield return null;
        }
    }

    void SetNextWaypoint()
    {
        currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        agent.SetDestination(waypoints[currentWaypoint].position);
    }
}