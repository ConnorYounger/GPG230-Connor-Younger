using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W5Projectile : MonoBehaviour
{
    public float damage = 4;
    public float projectileSpeed = 1;
    public float projectileTime = 5;

    public GameObject destroyFx;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip destroySound;

    private void Start()
    {
        if(projectileTime > 0)
            Destroy(gameObject, projectileTime);
    }

    void Update()
    {
        ProjectileMovement();
    }

    void ProjectileMovement()
    {
        transform.position += transform.forward * projectileSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<W5EnemyHealth>())
        {
            other.GetComponent<W5EnemyHealth>().DealDamage(damage);

            DestroyProjectile();
        }
    }

    void DestroyProjectile()
    {
        if (destroyFx)
        {
            GameObject fx = Instantiate(destroyFx, transform.position, transform.rotation);
            Destroy(fx, 1);
        }

        if (audioSource && destroySound)
        {
            audioSource.clip = destroySound;
            audioSource.Play();
        }

        Destroy(gameObject);
    }
}
