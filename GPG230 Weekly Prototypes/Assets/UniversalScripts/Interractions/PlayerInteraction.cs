using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Transform playerCam;
    public float interactDistance = 2;

    public WeaponManager weaponManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SearchForInterractable();
    }

    void SearchForInterractable()
    {
        RaycastHit hit;
        Physics.Raycast(playerCam.position, playerCam.forward, out hit, interactDistance, 7);

        Debug.DrawLine(playerCam.position, hit.point, Color.blue);

        if(hit.collider != null)
        {
            // hit something

            if (hit.collider.GetComponent<WeaponBase>())
            {
                WeaponInteraction(hit.collider.GetComponent<WeaponBase>());
            }
        }
    }

    void WeaponInteraction(WeaponBase weapon)
    {
        // outline weapon

        if (Input.GetKeyDown(KeyCode.E))
        {
            weaponManager.PickUpWeapon(weapon.gameObject);
        }
    }
}
