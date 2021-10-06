using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W2AudioManager : MonoBehaviour
{

    public AudioSource doorAudioSource;
    public AudioClip openDoorSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayOpenDoorSound()
    {
        doorAudioSource.clip = openDoorSound;
        doorAudioSource.Play();
    }
}
