using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInteraction : MonoBehaviour
{
    public W2Player player;

    public Color mouseOverColor = Color.blue;
    public Color searchDoorColor = Color.yellow;
    public Color searchItemColor = Color.green;

    private List<W2Interractable> items;
    private List<W2Door> doors;

    public bool caninterract = true;

    void Awake()
    {
        items = new List<W2Interractable>();
        doors = new List<W2Door>();
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (caninterract)
        {
            MouseClick();
            PlayerMovement();
        }
    }

    void MouseClick()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider != null && hit.collider.GetComponent<W2Interractable>())
            {
                hit.collider.GetComponent<W2Interractable>().MouseOver(mouseOverColor);
            }

            if (hit.collider != null && hit.collider.GetComponent<W2Door>())
            {
                hit.collider.GetComponent<W2Door>().MouseOver(mouseOverColor);
            }

            if (Input.GetButtonDown("Fire1"))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.GetComponent<W2Interractable>())
                    {
                        player.currentInterractable = hit.collider.GetComponent<W2Interractable>();
                        player.SetNewDestination(hit.collider.transform.position);
                        Debug.Log("Hit: " + hit.collider.name);

                        player.currentDoor = null;
                    }
                    else if (hit.collider.GetComponent<W2Door>())
                    {
                        player.currentDoor = hit.collider.GetComponent<W2Door>();
                        player.SetNewDestination(hit.collider.transform.position);

                        player.currentInterractable = null;
                    }
                    else
                    {
                        OffClick(true);
                    }
                }
                else
                {
                    OffClick(true);
                }
            }
        }
    }

    public void Search()
    {
        foreach(W2Interractable item in items)
        {
            if (item.gameObject.active && item.isItem)
                item.ShowOutline(searchItemColor);
            else
                item.ShowOutline(searchDoorColor);
        }

        foreach (W2Door door in doors)
        {
            if(door.gameObject.active)
                door.ShowOutline(searchDoorColor);
        }
    }

    public void AddItem(W2Interractable item)
    {
        items.Add(item);
    }

    public void AddDoor(W2Door door)
    {
        doors.Add(door);
    }

    void OffClick(bool resetDest)
    {
        player.currentDoor = null;
        player.currentInterractable = null;

        if(resetDest)
            player.SetNewDestination(player.transform.position);
    }

    void PlayerMovement()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100, ~6))
            {
                if (hit.collider != null)
                {
                    Debug.Log("Player Move to: " + hit.point);

                    player.SetNewDestination(hit.point);

                    OffClick(false);
                }
            }
        }
    }
}
