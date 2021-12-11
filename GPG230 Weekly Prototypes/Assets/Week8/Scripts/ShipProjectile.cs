using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShipProjectile : MonoBehaviour
{
    public int projectileDamage;
    public float projectileSpeed;
    public GameObject destroyEffect;

    public bool isHoming;
    public GameObject target;

    public float lifeTime = 1;
    private PhotonView photonView;

    void Start()
    {
        photonView = gameObject.GetComponent<PhotonView>();
    }

    void Update()
    {
        ProjectileMovement();
    }

    void ProjectileMovement()
    {
        transform.position += transform.forward * projectileSpeed * Time.deltaTime;

        if (isHoming)
        {
            if (target)
            {
                transform.LookAt(target.transform.position);
            }
            else
            {
                FindTarget();
            }
        }
    }

    void FindTarget()
    {
        if (EnemyShipSpawnManager.spawnedEnemies.Count > 0)
        {
            float dis = 10000;
            foreach (GameObject enemy in EnemyShipSpawnManager.spawnedEnemies)
            {
                if (Vector3.Distance(transform.position, enemy.transform.position) < dis)
                {
                    dis = Vector3.Distance(transform.position, enemy.transform.position);
                    target = enemy;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Hit enemy or player, deal damage

        if (other.GetComponent<EnemyShipHealth>())
        {
            other.GetComponent<EnemyShipHealth>().TakeDamage(projectileDamage);
        }
        else if (other.GetComponent<PlayerShipHealth>())
        {
            other.GetComponent<PlayerShipHealth>().TakeDamage(Mathf.RoundToInt(projectileDamage / 2));
        }

        if(photonView == null)
            DestroyProjectile();
        else
        {
            if (photonView.IsMine)
                DestroyProjectile();
        }
    }

    public IEnumerator ProjectileLifeTime()
    {
        yield return new WaitForSeconds(lifeTime);

        DestroyProjectile();
    }

    public void DestroyProjectile()
    {
        if (destroyEffect) 
        {
            if (photonView == null)
            {
                GameObject fx = Instantiate(destroyEffect, transform.position, transform.rotation);
                Destroy(fx, 3);
            }
            else
            {
                if(photonView.IsMine)
                    PhotonNetwork.Instantiate(destroyEffect.name, transform.position, transform.rotation);
            }
        }

        StopCoroutine("ProjectileLifeTime");
        if (photonView == null)
            Destroy(gameObject);
        else
            PhotonNetwork.Destroy(gameObject);
    }
}
