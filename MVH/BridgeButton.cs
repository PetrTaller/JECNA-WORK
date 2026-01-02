using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class BridgeButton : MonoBehaviour
{
    public GameObject bridge;
    public NavMeshSurface navSurface;

    void Start()
    {
        bridge.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bridge.SetActive(true);
            navSurface.BuildNavMesh();
        }
    }

}

