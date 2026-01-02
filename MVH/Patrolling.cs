using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrolling : MonoBehaviour
{

    public Transform[] waypoints;
    private int currentWaypoint = 0;
    public float followDistance = 10f;
    public float returnDistance = 15f;
    NavMeshAgent agent;
    private GameObject player;

    void Start()
    {
        transform.position = waypoints[currentWaypoint].position;
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        float distanceToWaypoint = Vector3.Distance(transform.position, waypoints[currentWaypoint].position);

        if (distanceToPlayer <= followDistance)
        {
            agent.SetDestination(player.transform.position);
        }
        else if (distanceToPlayer >= returnDistance)
        {
            agent.SetDestination(waypoints[currentWaypoint].position);
            if (distanceToWaypoint <= 1)
            {

                currentWaypoint++;
                if (currentWaypoint == waypoints.Length)
                {
                    currentWaypoint = 0;
                }
            }
        }
    }
}
