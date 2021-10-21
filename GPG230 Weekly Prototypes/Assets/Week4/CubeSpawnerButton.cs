using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawnerButton : MonoBehaviour
{
    public CubeSpawner cubeSpawner;
    public Animator animator;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonPress()
    {
        if (cubeSpawner)
        {
            cubeSpawner.SpawnNewCube();
        }

        if (animator)
        {
            animator.Play("ButtonPress");
        }

        if (audioSource)
        {
            audioSource.Play();
        }
    }
}
