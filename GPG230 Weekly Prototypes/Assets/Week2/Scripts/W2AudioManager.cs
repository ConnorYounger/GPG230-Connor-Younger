using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W2AudioManager : MonoBehaviour
{
    [Header("Rain")]
    public AudioSource rainAudioSource;
    public AudioClip rainClip;
    public float[] rainRoomVolumes;

    [Header("Doors")]
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

    public void AdjustRainAudio(int roomIndex)
    {
        switch (roomIndex)
        {
            case 12:
                rainAudioSource.volume = rainRoomVolumes[3];
                break;
            case 7:
                rainAudioSource.volume = rainRoomVolumes[2];
                break;
            case 2:
                rainAudioSource.volume = rainRoomVolumes[1];
                break;
            case 6:
                rainAudioSource.volume = rainRoomVolumes[1];
                break;
            case 10:
                rainAudioSource.volume = rainRoomVolumes[1];
                break;
            default:
                rainAudioSource.volume = rainRoomVolumes[0];
                break;
        }
    }
}
