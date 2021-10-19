using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCheckpoint : MonoBehaviour
{
    private bool hasSetCheckPoint;
    public Transform spawnPoint;
    public PuzzleCheckPointManager manager;

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player" && !hasSetCheckPoint)
        {
            hasSetCheckPoint = true;

            manager.SetNewCheckPoint(spawnPoint);
        }
    }
}
