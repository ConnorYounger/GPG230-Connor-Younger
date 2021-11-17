using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShipHealth : MonoBehaviour
{
    public int startingHealth = 20;
    public int currentHealth;

    public EnemyShipSpawnManager enemyShipManager;
    public Rigidbody rb;

    public Slider healthSlider;

    [Header("DeathRefs")]
    public GameObject destroyFx;
    public W8ShipMovement shipMovement;
    public GameObject shipChildMesh;
    public BoxCollider boxCollider;
    public GameObject destroySound;

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

        if (healthSlider)
        {
            healthSlider.value = currentHealth / startingHealth;
            Debug.Log(currentHealth / startingHealth);
        }

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

        if (destroySound)
        {
            GameObject sfx = Instantiate(destroySound, transform.position, transform.rotation);
            Destroy(sfx, 5);
        }

        shipMovement.enabled = false;
        shipChildMesh.SetActive(false);
        boxCollider.enabled = false;
        enemyShipManager.PlayerDeath();
        enemyShipManager.scenarioManager.ShowLoseUI();
        this.enabled = false;
    }
}
