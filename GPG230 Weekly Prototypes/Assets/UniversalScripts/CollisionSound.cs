using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    public bool canCreateSounds = true;
    public AudioSource audioSource;
    public AudioClip[] sounds;

    public bool modifyVolumeProperties;
    public float volume = 1;
    public float minDistance = 1;
    public float maxDistance = 20;

    void Start()
    {
        if (!audioSource)
            audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(audioSource && canCreateSounds)
            CreateSound();
    }

    void CreateSound()
    {
        if (sounds.Length > 0)
        {
            int rand = Random.Range(0, sounds.Length);
            audioSource.clip = sounds[rand];

            if (modifyVolumeProperties)
            {
                audioSource.volume = volume;
                audioSource.minDistance = minDistance;
                audioSource.maxDistance = maxDistance;
            }

            audioSource.Play();
        }
    }
}
