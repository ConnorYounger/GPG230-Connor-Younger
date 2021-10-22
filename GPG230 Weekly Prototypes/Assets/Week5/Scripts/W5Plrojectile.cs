using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W5Plrojectile : MonoBehaviour
{
    public float projectileSpeed = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
        //if(other.GetComponent<>())
    }
}
