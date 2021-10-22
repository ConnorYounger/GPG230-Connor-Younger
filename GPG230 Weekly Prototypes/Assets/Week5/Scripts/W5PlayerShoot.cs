using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W5PlayerShoot : MonoBehaviour
{
    public GameObject projectile1;
    public Transform shootPoint;

    public float fireRate;
    private bool readyToFire = true;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerAiming();
        PlayerShooting();
    }

    void PlayerAiming()
    {
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
        float hitDist = 0;

        if(playerPlane.Raycast(ray, out hitDist))
        {
            Vector3 targetPoint = ray.GetPoint(hitDist);
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            targetRotation.x = 0;
            targetRotation.z = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 7 * Time.deltaTime);
        }
    }

    void PlayerShooting()
    {
        if (readyToFire)
        {
            if (Input.GetButton("Fire1"))
            {
                if (projectile1 && shootPoint)
                {
                    GameObject proj = Instantiate(projectile1, shootPoint.position, shootPoint.rotation);

                    readyToFire = false;

                    StopCoroutine("ResetFireRate");
                    StartCoroutine("ResetFireRate");
                }
            }
        }
    }

    IEnumerator ResetFireRate()
    {
        yield return new WaitForSeconds(fireRate);

        readyToFire = true;
    }
}
