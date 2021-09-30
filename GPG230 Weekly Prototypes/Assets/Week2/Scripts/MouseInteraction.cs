using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInteraction : MonoBehaviour
{
    public W2Player player;

    public Color mouseOverColor = Color.blue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MouseClick();
        PlayerMovement();
    }

    void MouseClick()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider != null && hit.collider.GetComponent<W2Item>())
            {
                hit.collider.GetComponent<W2Item>().MouseOver(mouseOverColor);
            }

            if (hit.collider != null && hit.collider.GetComponent<W2Door>())
            {
                hit.collider.GetComponent<W2Door>().MouseOver(mouseOverColor);
            }

            if (Input.GetButtonDown("Fire1"))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.GetComponent<W2Item>())
                    {
                        player.currentItem = hit.collider.GetComponent<W2Item>();
                        player.SetNewDestination(hit.point);
                        Debug.Log("Hit: " + hit.collider.name);

                        player.currentDoor = null;
                    }
                    else if (hit.collider.GetComponent<W2Door>())
                    {
                        player.currentDoor = hit.collider.GetComponent<W2Door>();
                        player.SetNewDestination(hit.point);

                        player.currentItem = null;
                    }
                    else
                    {
                        OffClick();
                    }
                }
                else
                {
                    OffClick();
                }
            }
        }
    }

    void OffClick()
    {
        player.currentDoor = null;
        player.currentItem = null;
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

                    OffClick();
                }
            }
        }
    }
}
