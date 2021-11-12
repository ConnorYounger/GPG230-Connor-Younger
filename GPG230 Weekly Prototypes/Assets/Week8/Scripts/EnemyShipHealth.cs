using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipHealth : MonoBehaviour
{
    public int startingHealh = 100;
    public int currentHealth;

    public GameObject destroyFx;
    public EnemyShipSpawnManager spawnManager;

    void Start()
    {
        currentHealth = startingHealh;
    }

    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die() 
    {
        if (destroyFx)
        {
            GameObject fx = Instantiate(destroyFx, transform.position, transform.rotation);
            Destroy(fx, 5);
        }

        spawnManager.RemoveShip(gameObject);

        Destroy(gameObject);
    }
}
