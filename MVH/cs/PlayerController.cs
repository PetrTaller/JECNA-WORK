using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int speed = 3;
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        float vertical = Input.GetAxis("Vertical") * Time.deltaTime * speed;


        transform.Translate(0, 0, vertical);
        transform.Rotate(0, horizontal, 0);
        
    }
}
