using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

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

    [Header("Multiplayer")]
    public int respawnTime = 3;
    private int respawntimer;

    [Header("MultiplayerDeathRefs")]
    public GameObject multiDeathUI;
    public TMP_Text respawnCountDownText;

    [Header("Collision Sounds")]
    public AudioSource as1;
    public AudioSource as2;
    public AudioClip[] sounds1;
    public AudioClip[] sounds2;

    public PhotonView photonView;

    private bool isAlive = true;
    private MultiplayerScenarioManager multiplayerManager;
    public PhotonPlayerManager lastHitPlayer;

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
            multiDeathUI = GameObject.Find("MultiplayerDeathUI");
            multiDeathUI.SetActive(false);
            multiplayerManager = GameObject.Find("MultiplayerManager").GetComponent<MultiplayerScenarioManager>();

            if(GameObject.Find("MultiplayerRespawningText"))
                respawnCountDownText = GameObject.Find("MultiplayerRespawningText").GetComponent<TMP_Text>();
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

        if (photonView == null)
        {
            if (enemyShipManager)
            {
                enemyShipManager.PlayerDeath();
                enemyShipManager.scenarioManager.ShowLoseUI();
                this.enabled = false;
            }
        }
        else
        {
            if (photonView.IsMine)
            {
                respawntimer = respawnTime;
                StartCoroutine("RespawnCountDown");

                if (lastHitPlayer)
                {
                    multiplayerManager.photonView.RPC("PlayerGotKilled", RpcTarget.AllBuffered, photonView.ViewID, lastHitPlayer.photonView.ViewID);
                }
            }
        }
    }

    IEnumerator RespawnCountDown()
    {
        if(respawntimer <= 0)
        {
            Respawn();
            //photonView.RPC("Respawn", RpcTarget.All);
        }
        else
        {
            multiDeathUI.SetActive(true);

            if (respawnCountDownText == null)
                respawnCountDownText = GameObject.Find("MultiplayerRespawningText").GetComponent<TMP_Text>();

            respawnCountDownText.text = "Respawning in " + respawntimer + "s";
        }

        yield return new WaitForSeconds(1);

        respawntimer--;

        if(respawntimer >= 0)
            StartCoroutine("RespawnCountDown");
    }

    [PunRPC]
    public void Respawn()
    {
        currentHealth = startingHealth;

        shipMovement.enabled = true;
        shipChildMesh.SetActive(true);
        boxCollider.enabled = true;

        isAlive = true;
        multiDeathUI.SetActive(false);
        // Respawn Point

        if (healthSlider)
        {
            healthSlider.value = currentHealth;
        }

        int rand = Random.Range(0, multiplayerManager.spawnPoints.Length);
        transform.position = multiplayerManager.spawnPoints[rand].position;
        transform.rotation = multiplayerManager.spawnPoints[rand].rotation;

        StopCoroutine("RespawnCountDown");
    }
}
