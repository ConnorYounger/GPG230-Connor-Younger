using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

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

    [Header("Collision Sounds")]
    public AudioSource as1;
    public AudioSource as2;
    public AudioClip[] sounds1;
    public AudioClip[] sounds2;

    public PhotonView photonView;

    private bool isAlive = true;

    void Start()
    {
        photonView = gameObject.GetComponent<PhotonView>();

        currentHealth = startingHealth;

        if(GameObject.Find("EnemySpawnManager"))
            if (enemyShipManager = GameObject.Find("EnemySpawnManager").GetComponent<EnemyShipSpawnManager>()) { }

        if (healthSlider)
        {
            healthSlider.maxValue = startingHealth;
            healthSlider.value = currentHealth;
        }

        if(photonView && photonView.IsMine)
        {
            healthSlider = GameObject.Find("PlayerHealthSlider").GetComponent<Slider>();
        }
    }

    public void SetStartingHealth(int health)
    {
        startingHealth = health;
        currentHealth = startingHealth;

        if (healthSlider)
        {
            healthSlider.maxValue = startingHealth;
            healthSlider.value = currentHealth;
        }
    }

    void Update()
    {
        
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (healthSlider)
        {
            healthSlider.value = currentHealth;
        }

        if(currentHealth <= 0 && isAlive)
        {
            Die();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayColissionSound();

        if(rb && rb.velocity.magnitude > 50)
        {
            TakeDamage(Mathf.RoundToInt(rb.velocity.magnitude / 5));
        }
    }

    void PlayColissionSound()
    {
        PlayRandSound.PlayRandomSound(as1, sounds1);
        PlayRandSound.PlayRandomSound(as2, sounds2);
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
        if (enemyShipManager)
        {
            enemyShipManager.PlayerDeath();
            enemyShipManager.scenarioManager.ShowLoseUI();
        }
        this.enabled = false;
    }
}
