using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInteraction : MonoBehaviour
{
    public W2Player player;

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
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider != null)
                {
                    Debug.Log("Hit: " + hit.collider.name);
                }
            }
        }
    }

    void PlayerMovement()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100, 6))
            {
                if (hit.collider != null)
                {
                    player.SetNewDestination(hit.point);
                }
            }
        }
    }
}
