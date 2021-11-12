using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipProjectile : MonoBehaviour
{
    public float projectileSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position += transform.forward * projectileSpeed * Time.deltaTime;
    }
}
