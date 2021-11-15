using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipHealth : MonoBehaviour
{
    public int startingHealh = 100;
    public int currentHealth;

    public GameObject destroyFx;
    public EnemyShipSpawnManager spawnManager;
    public EnemyShipAI shipAI;

    public GameObject fireEffect;
    private GameObject spawnedFireEffect;

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

        if(currentHealth <= startingHealh / 2)
        {
            if (fireEffect && !spawnedFireEffect)
            {
                spawnedFireEffect = Instantiate(fireEffect, transform.position, transform.rotation);
                spawnedFireEffect.transform.parent = transform;
            } 

            if (currentHealth <= 0)
            {
                Die();
            }
        }
        else
        {
            if(shipAI && !shipAI.isTriggered)
            {
                shipAI.isTriggered = true;
            }
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
