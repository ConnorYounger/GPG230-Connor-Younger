using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W5EnemyShooting : MonoBehaviour
{
    public GameObject projectile;

    public Transform aimTransform;
    public Transform shootPoint;
    private Transform player;

    public float damage = 4;
    public float projectileSpeed = 3;

    public float fireRate;
    public int band;

    private bool readyToFire = true;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player)
            EnemyAiming();

        //EnemyShooting();
        AudioEnemyShooting();
    }

    void EnemyAiming()
    {
        Quaternion targetRotation = Quaternion.LookRotation(player.position - transform.position);
        targetRotation.x = 0;
        targetRotation.z = 0;
        //aimTransform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 7 * Time.deltaTime);
        aimTransform.LookAt(player);
    }

    void EnemyShooting()
    {
        if (readyToFire)
        {
            if (projectile && shootPoint)
            {
                GameObject proj = Instantiate(projectile, shootPoint.position, shootPoint.rotation);

                if (proj.GetComponent<W5Projectile>())
                {
                    proj.GetComponent<W5Projectile>().damage = damage;
                    proj.GetComponent<W5Projectile>().projectileSpeed = projectileSpeed;
                }

                readyToFire = false;

                StopCoroutine("ResetFireRate");
                StartCoroutine("ResetFireRate");
            }
        }
    }

    void AudioEnemyShooting()
    {
        if (readyToFire)
        {
            if (projectile && shootPoint && AudioPeer.audioBandBuffer[band] > 0.7f)
            {
                GameObject proj = Instantiate(projectile, shootPoint.position, shootPoint.rotation);

                if (proj.GetComponent<W5Projectile>())
                {
                    proj.GetComponent<W5Projectile>().damage = damage;
                    proj.GetComponent<W5Projectile>().projectileSpeed = projectileSpeed;
                }

                readyToFire = false;

                StopCoroutine("ResetFireRate");
                StartCoroutine("ResetFireRate");
            }
        }
    }

    IEnumerator ResetFireRate()
    {
        yield return new WaitForSeconds(fireRate);

        readyToFire = true;
    }
}
