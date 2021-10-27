using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportZone : MonoBehaviour
{
    public Transform teleportPoint;

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            CharacterController con = other.GetComponent<CharacterController>();

            if (con != null)
                con.enabled = false;

            other.transform.position = teleportPoint.position;

            if (con != null)
                con.enabled = true;
        }
    }
}
