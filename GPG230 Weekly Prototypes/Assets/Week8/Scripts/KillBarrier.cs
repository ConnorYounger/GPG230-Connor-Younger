using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBarrier : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerShipHealth>())
        {
            other.GetComponent<PlayerShipHealth>().TakeDamage(other.GetComponent<PlayerShipHealth>().startingHealth);
        }
    }
}
