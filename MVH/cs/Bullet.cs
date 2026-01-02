using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody _body;
    private float _speed = 20f;

    void Start()
    {
        _body = GetComponent<Rigidbody>();
        _body.velocity = transform.forward * _speed;
    }

    // Update is called once per frame

    
    void OnTriggerEnter(Collider collision)
    {
        Obstacle enemy = collision.GetComponent<Obstacle>();
        if (enemy)
        {
            enemy.TakeDamage(5f);
        }

        if (collision.tag != "Player")
        {
            Destroy(this.gameObject);
        }
    }

}
