using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedPuzzleObject : MonoBehaviour
{
    public CubeSpawner spawner;
    private PreasurePad preasurePad;

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
                DestroyObject();
        }

        if (other.GetComponent<PreasurePad>())
        {
            preasurePad = other.GetComponent<PreasurePad>();
        }
    }

    public void DestroyObject()
    {
        if (preasurePad && gameObject.GetComponent<BoxCollider>())
        {
            preasurePad.RemoveObject(gameObject.GetComponent<BoxCollider>());
        }

        Destroy(gameObject);
    }
}
