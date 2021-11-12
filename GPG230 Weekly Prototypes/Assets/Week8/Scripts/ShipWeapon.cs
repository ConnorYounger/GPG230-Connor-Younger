using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipWeapon : MonoBehaviour
{
    public ShipWeaponStats weapon;

    public Transform shootPoint;

    public bool canFire = true;

    private Quaternion defultAimPos;

    // Start is called before the first frame update
    void Start()
    {
        defultAimPos = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        WeaponAiming();
    }

    void WeaponAiming()
    {
        Vector3 lookPoint = new Vector3();
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000, ~8))
        {
            if (hit.collider != null)
            {
                lookPoint = hit.point;
            }
            else
            {
                //Plane playerPlane = new Plane(Vector3.up, transform.position);
                //Ray ray2 = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
                //float hitDist = 1000;

                //if (playerPlane.Raycast(ray2, out hitDist))
                //{
                //    lookPoint = ray2.GetPoint(hitDist);
                //}

                lookPoint = transform.parent.forward;
            }
        }

        transform.LookAt(lookPoint);
    }
}
