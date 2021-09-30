using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class W2Player : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private Vector3 destinationPoint;

    // Start is called before the first frame update
    void Start()
    {
        navAgent = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    public void SetNewDestination(Vector3 position)
    {
        destinationPoint = position;
    }

    void MovePlayer()
    {
        navAgent.SetDestination(destinationPoint);
    }
}
