using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class W2Player : MonoBehaviour
{
    public NavMeshAgent navAgent;
    private Vector3 destinationPoint;

    public W2Inventory inventory;

    public W2Interractable currentInterractable;
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
        navAgent.enabled = true;
        destinationPoint = position;
    }

    void MovePlayer()
    {
        if(navAgent.enabled)
            navAgent.SetDestination(destinationPoint);
    }

    void ItemDistanceCheck()
    {
        if(currentInterractable != null)
        {
            if(Vector3.Distance(transform.position, currentInterractable.transform.position) < interractDistance)
            {
                if (currentInterractable.isItem)
                    CollectItem();
                else
                    Interraction();
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
        if (currentInterractable != null)
        {
            inventory.PickUpItem(currentInterractable);

            currentInterractable = null;
        }
    }

    void Interraction()
    {
        if(currentInterractable != null)
        {
            switch (currentInterractable.interractableType.ToString())
            {
                case "frontDoor":
                    if(inventory.key != null)
                    {
                        Debug.Log("Front door win");
                    }
                    else
                    {
                        Debug.Log("The door is locked, maybe there's a key around here somewhere");
                    }
                    break;
                case "window":
                    if (inventory.axe != null)
                    {
                        Debug.Log("Window win");
                    }
                    else
                    {
                        Debug.Log("Maybe I can break through this window with a weapon");
                    }
                    break;
                case "highWindow":
                    if (inventory.ladder != null)
                    {
                        Debug.Log("High window win");
                    }
                    else
                    {
                        Debug.Log("If only I can find something to reach that window");
                    }
                    break;
                case "workBench":
                    break;
            }
        }
    }
}
