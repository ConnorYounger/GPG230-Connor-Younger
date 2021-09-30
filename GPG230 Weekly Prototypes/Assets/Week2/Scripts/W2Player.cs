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
    public W2Door currentDoor;

    public float interractDistance = 0.8f;

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
        DoorDistanceCheck();
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
            if(Vector3.Distance(transform.position, currentItem.transform.position) < interractDistance)
            {
                CollectItem();
            }
        }
    }

    void DoorDistanceCheck()
    {
        if (currentDoor != null)
        {
            if (Vector3.Distance(transform.position, currentDoor.transform.position) < 0.8f)
            {
                UseDoor();
            }
        }
    }

    void UseDoor()
    {
        if(currentDoor != null)
        {
            currentDoor.UseDoor();

            currentDoor = null;
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
