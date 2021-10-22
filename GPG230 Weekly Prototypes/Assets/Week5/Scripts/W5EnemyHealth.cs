using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W5EnemyHealth : MonoBehaviour
{
    public float health = 10;

    public GameObject destroyFx;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip destroySound;

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
    }

    void EnemyDie()
    {
        if (destroyFx)
        {
            GameObject fx = Instantiate(destroyFx, transform.position, transform.rotation);
            Destroy(fx, 1);
        }

        if(audioSource && destroySound)
        {
            audioSource.clip = destroySound;
            audioSource.Play();
        }

        Destroy(gameObject);
    }
}
