using System.Diagnostics;
using UnityEngine;

public class ExampleClass : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ExecuteFunction();
        }
    }

    void ExecuteFunction()
    {
        // Your custom function logic goes here
        Debug.Log("Function executed!");
    }
}