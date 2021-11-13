using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipHealth : MonoBehaviour
{
    public int startingHealth = 20;
    public int currentHealth;

    public EnemyShipSpawnManager enemyShipManager;
    public Rigidbody rb;

    [Header("DeathRefs")]
    public GameObject destroyFx;
    public W8ShipMovement shipMovement;
    public GameObject shipChildMesh;
    public BoxCollider boxCollider;

    private bool isAlive = true;

    void Start()
    {
        currentHealth = startingHealth;

        enemyShipManager = GameObject.Find("EnemySpawnManager").GetComponent<EnemyShipSpawnManager>();
    }

    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0 && isAlive)
        {
            Die();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(rb && rb.velocity.magnitude > 50)
        {
            TakeDamage(Mathf.RoundToInt(rb.velocity.magnitude / 5));
        }
    }

    void Die()
    {
        isAlive = false;

        Debug.Log("PlayerDeath");

        if (destroyFx)
        {
            GameObject fx = Instantiate(destroyFx, transform.position, transform.rotation);
            Destroy(fx, 5);
        }

        shipMovement.enabled = false;
        shipChildMesh.SetActive(false);
        boxCollider.enabled = false;
        enemyShipManager.PlayerDeath();
        this.enabled = false;
    }
}
