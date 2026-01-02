using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class bakeMesh : MonoBehaviour
{
    public GameObject bridge;
    public NavMeshSurface navSurface;
    // Start is called before the first frame update
    void Start()
    {
        bridge.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bridge.SetActive(true);
            navSurface.BuildNavMesh();
        }
    }
}
