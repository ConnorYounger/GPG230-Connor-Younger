using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W5PlayerShoot : MonoBehaviour
{
    public GameObject projectile1;
    public Transform shootPoint;

    public float damage = 4;
    public float projectileSpeed = 7;

    public int fireRate = 1;
    private int fireRateCounter;
    private bool readyToFire = true;

    [Header("Aduio")]
    public AudioSource audioSource;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerAiming();
        PlayerShooting();
    }

    private void FixedUpdate()
    {
        if (!readyToFire && BPeerM.beatD8)
        {
            if (fireRateCounter >= fireRate)
            {
                readyToFire = true;
                fireRateCounter = 0;
            }
            else
            {
                fireRateCounter++;
            }
        }
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

                    if (proj.GetComponent<W5Projectile>())
                    {
                        proj.GetComponent<W5Projectile>().damage = damage;
                        proj.GetComponent<W5Projectile>().projectileSpeed = projectileSpeed;
                    }

                    readyToFire = false;

                    if (audioSource)
                    {
                        audioSource.Play();
                    }

                    //StopCoroutine("ResetFireRate");
                    //StartCoroutine("ResetFireRate");
                }
            }
        }
    }

    //IEnumerator ResetFireRate()
    //{
    //    yield return new WaitForSeconds(fireRate);

    //    readyToFire = true;
    //}
}
