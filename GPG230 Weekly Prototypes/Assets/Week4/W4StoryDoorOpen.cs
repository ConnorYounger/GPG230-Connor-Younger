using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W4StoryDoorOpen : MonoBehaviour
{
    public bool distance = true;
    public W3Door door;
    
    private void OnTriggerEnter(Collider other)
    {
        if (distance)
        {
            door.OpenDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (distance)
        {
            door.CloseDoor();
        }
    }
}
