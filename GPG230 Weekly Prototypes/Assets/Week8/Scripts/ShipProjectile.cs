using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipProjectile : MonoBehaviour
{
    public float projectileSpeed;
    public GameObject destroyEffect;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position += transform.forward * projectileSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Hit enemy or player, deal damage

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
