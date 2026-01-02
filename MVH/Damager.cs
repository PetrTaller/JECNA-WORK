using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    private IEnumerator damageCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            damageCoroutine = DamagePlayer(other);
            StartCoroutine(damageCoroutine);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StopCoroutine(damageCoroutine);

        }
    }
    private IEnumerator DamagePlayer(Collider player)
    {
        while (true)
        {

            player.GetComponent<Health>().TakeDamage(10f);

            yield return new WaitForSeconds(2f);
        }

    }
}

