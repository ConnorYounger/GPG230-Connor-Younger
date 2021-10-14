using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;
//using StatePattern;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    public float healthRegenMultiplier;
    private bool canHeal = true;

    public float headShotMultiplier = 2;

    //public EnemySpawManager spawManager;
    //public GameManager gameManager;

    //public Enemy enemy;

    public MeshRenderer meshRenderer;

    public ParticleSystem[] dmgeffects;

    public bool isDead;

    [Header("Audio")]
    private AudioSource audioSource;
    public AudioClip damageSound;
    public AudioClip[] deathSound;

    [Space()]
    public Animator animator;
    public Rigidbody[] limbs;
    public GameObject zombieMesh;
    public CapsuleCollider capsuleCollider;
    public BoxCollider[] boxColliders;


    [Header("Hit fx")]
    public GameObject defultHitFx;
    public GameObject specialHitFx;

    void Start()
    {
        currentHealth = maxHealth;

        audioSource = gameObject.GetComponent<AudioSource>();

        if (limbs.Length > 0)
        {
            for (int i = 0; i < limbs.Length; i++)
            {
                limbs[i].constraints = RigidbodyConstraints.FreezeAll;
            }
        }

        //if (!gameManager)
        //    gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        AutoHeal();
    }

    void AutoHeal()
    {
        if(canHeal && healthRegenMultiplier > 0 && !isDead)
        {
            if(currentHealth < maxHealth)
            {
                currentHealth += 10 * healthRegenMultiplier * Time.deltaTime;
            }
            else if(currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (!isDead)
        {
            currentHealth -= damage;

            //if(damageSound)
            //    PlaySound(damageSound);

            AddScore(Mathf.RoundToInt(damage - (damage / 3)));

            if (currentHealth > 0)
            {
                StopCoroutine("AutoHealCoolDown");
                StartCoroutine("AutoHealCoolDown");

                //if (enemy)
                //{
                //    enemy.hasFoundPlayer = true;
                //    enemy.PlayHitSound();
                //}

                if(dmgeffects.Length > 0)
                {
                    foreach(ParticleSystem system in dmgeffects)
                    {
                        system.Play();
                    }
                }
            }
            else
                Die();
        }
    }

    public void AddScore(int value)
    {
        //if (gameManager)
        //{
        //    gameManager.AddScore(value);
        //}
    }

    IEnumerator AutoHealCoolDown()
    {
        canHeal = false;

        yield return new WaitForSeconds(2);

        canHeal = true;
    }

    public void Die()
    {
        isDead = true;

        Debug.Log("EnemyDie");

        //if (spawManager)
        //{
        //    spawManager.KilledEnemy(gameObject);
        //}

        //if (enemy)
        //{
        //    enemy.PlayHitSound();
        //    enemy.enabled = false;

        //    if (gameObject.GetComponent<NavMeshAgent>())
        //        gameObject.GetComponent<NavMeshAgent>().enabled = false;

        //    if (gameObject.GetComponent<Rigidbody>())
        //    {
        //        //Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        //        //rb.constraints = RigidbodyConstraints.None;
        //    }

        //    if (gameObject.GetComponent<BoxCollider>())
        //        gameObject.GetComponent<BoxCollider>().enabled = false;
        //}

        if (meshRenderer)
        {
            meshRenderer.material.color = Color.red;
        }

        //if (audioSource && deathSound.Length > 0)
        //{
        //    int rand = Random.Range(0, deathSound.Length);
        //    audioSource.clip = deathSound[rand];
        //    audioSource.Play();
        //}

        if(limbs.Length > 0)
        {
            for(int i = 0; i < limbs.Length; i++)
            {
                limbs[i].constraints = RigidbodyConstraints.None;
            }
        }

        if (animator)
        {
            //animator.enabled = false;
            animator.SetBool("isDead", true);
        }

        if (zombieMesh)
        {
            //zombieMesh.SetActive(false);
        }

        if (capsuleCollider)
        {
            capsuleCollider.enabled = false;
        }

        if(boxColliders.Length > 0)
        {
            foreach(BoxCollider collider in boxColliders)
            {
                collider.enabled = false;
            }
        }

        StartCoroutine("DisableAudioSource");
    }

    //IEnumerator DisableAudioSource()
    //{
    //    yield return new WaitForSeconds(enemy.sfxTIme);

    //    if(enemy.audioSource)
    //        enemy.audioSource.enabled = false;
    //}

    public void PlaySound(AudioClip sound)
    {
        GameObject soundOb = Instantiate(new GameObject(), transform.position, transform.rotation);
        AudioSource aSource = soundOb.AddComponent<AudioSource>();

        aSource.volume = PlayerPrefs.GetFloat("audioVolume");
        aSource.spatialBlend = 1;
        aSource.maxDistance = 100;
        aSource.clip = sound;
        aSource.Play();

        Destroy(soundOb, sound.length);
    }


    public GameObject GetHitFx(string fx)
    {
        if (fx == "defult")
        {
            return defultHitFx;
        }
        else if (fx == "special")
        {
            return specialHitFx;
        }
        else
        {
            return null;
        }
    }
}
