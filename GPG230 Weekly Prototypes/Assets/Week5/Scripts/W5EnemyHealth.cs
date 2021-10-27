using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W5EnemyHealth : MonoBehaviour
{
    public float health = 10;

    public GameObject hitFx;
    public GameObject destroyFx;

    public W5EnemySpawner spawner;
    public int spawnIndex = -1;

    public int scoreValue = 10;

    [Header("Audio")]
    //public AudioSource audioSource;
    public AudioClip[] destroySounds;

    public void DealDamage(float damage)
    {
        health -= damage;

        CheckForDeath();
    }

    void CheckForDeath()
    {
        if(health <= 0)
        {
            EnemyDie();
        }
        else
        {
            if (hitFx)
            {
                GameObject fx = Instantiate(hitFx, transform.position, transform.rotation);
                Destroy(fx, 1);
            }
        }
    }

    void EnemyDie()
    {
        if (spawner)
        {
            spawner.RemoveEnemy(gameObject, spawnIndex, scoreValue);
        }

        if (destroyFx)
        {
            GameObject fx = Instantiate(destroyFx, transform.position, transform.rotation);
            Destroy(fx, 1);
        }

        if(destroySounds.Length > 0)
        {
            int rand = Random.Range(0, destroySounds.Length);

            AudioSource.PlayClipAtPoint(destroySounds[rand], transform.position, 1);
        }

        Destroy(gameObject);
    }
}
