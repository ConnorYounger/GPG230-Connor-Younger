using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] sounds;

    void Start()
    {
        PlayRandomSound();
    }

    void PlayRandomSound()
    {
        if(audioSource && sounds.Length > 0)
        {
            PlayRandomSound(audioSource, sounds);
        }
    }

    public static void PlayRandomSound(AudioSource aS, AudioClip[] aC)
    {
        int rand = Random.Range(0, aC.Length);
        aS.clip = aC[rand];
        aS.Play();
    }
}
