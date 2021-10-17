using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedPuzzleObject : MonoBehaviour
{
    public CubeSpawner spawner;

    public void SpawnNewObject()
    {
        spawner.SpawnNewCube();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PhizDoor>())
        {
            if (spawner)
                spawner.SpawnNewCube();
            else
                Destroy(gameObject);
        }
    }
}
