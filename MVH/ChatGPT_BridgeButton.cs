using UnityEngine;

public class BridgeButton : MonoBehaviour
{
    public GameObject bridge;
    public GameObject enemy;
    public NavMeshSurface navMeshSurface;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bridge.SetActive(true);
            navMeshSurface.BuildNavMesh();
            enemy.GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}