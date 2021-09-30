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
        if(Vector3.Distance(transform.position, destinationPoint) < 0.1f)
        {
            navAgent.enabled = true;
            navAgent.SetDestination(destinationPoint);
        }
        else
        {
            navAgent.enabled = false;
        }
    }
}
