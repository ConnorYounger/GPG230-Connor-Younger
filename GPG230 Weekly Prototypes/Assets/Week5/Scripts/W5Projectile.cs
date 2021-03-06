using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W5Projectile : MonoBehaviour
{
    public float damage = 4;
    public float projectileSpeed = 1;
    public float projectileTime = 5;

    public GameObject destroyFx;

    public W5ScoreManager scoreManager;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip destroySound;

    public bool musicScale;
    public float scaleMultiplier = 1;
    public int band;

    private void Start()
    {
        if(projectileTime > 0)
            Destroy(gameObject, projectileTime);

        scoreManager = GameObject.Find("ScoreManager").GetComponent<W5ScoreManager>();
    }

    void Update()
    {
        ProjectileMovement();
        MusicScale();
    }

    void ProjectileMovement()
    {
        transform.position += transform.forward * projectileSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if(other.GetComponent<W5EnemyHealth>())
                other.GetComponent<W5EnemyHealth>().DealDamage(damage);

            if (other.name == "Player" && scoreManager)
                scoreManager.RemoveScore(100);

            if (other.gameObject.layer != 12 && other.gameObject.layer != 14)
                DestroyProjectile();
        }
    }

    void MusicScale()
    {
        if (musicScale)
        {
            float locScale = Mathf.Clamp(AudioPeer.audioBandBuffer[band] * scaleMultiplier, scaleMultiplier / 2, scaleMultiplier);
            transform.localScale = new Vector3(locScale, locScale, locScale);
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
