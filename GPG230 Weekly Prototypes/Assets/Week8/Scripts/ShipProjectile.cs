using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipProjectile : MonoBehaviour
{
    public int projectileDamage;
    public float projectileSpeed;
    public GameObject destroyEffect;

    public bool isHoming;
    public GameObject target;

    void Start()
    {
        
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

        DestroyProjectile();
    }

    public void DestroyProjectile()
    {
        if (destroyEffect) 
        {
            GameObject fx = Instantiate(destroyEffect, transform.position, transform.rotation);
            Destroy(fx, 3);
        }

        Destroy(gameObject);
    }
}
