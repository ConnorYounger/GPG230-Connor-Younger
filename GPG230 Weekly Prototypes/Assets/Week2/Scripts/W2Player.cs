using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class W2Player : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private Vector3 destinationPoint;

    public W2Inventory inventory;

    public W2Item currentItem;

    // Start is called before the first frame update
    void Start()
    {
        navAgent = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        ItemDistanceCheck();
    }

    public void SetNewDestination(Vector3 position)
    {
        destinationPoint = position;
    }

    void MovePlayer()
    {
        navAgent.SetDestination(destinationPoint);
    }

    void ItemDistanceCheck()
    {
        if(currentItem != null)
        {
            Debug.Log(Vector3.Distance(transform.position, currentItem.transform.position));

            if(Vector3.Distance(transform.position, currentItem.transform.position) < 0.8f)
            {
                CollectItem();
            }
        }
    }

    void CollectItem()
    {
        if (currentItem != null)
        {
            inventory.PickUpItem(currentItem);

            currentItem = null;
        }
    }
}
