using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedPuzzleObject : MonoBehaviour
{
    public CubeSpawner spawner;
    private PreasurePad preasurePad;

    public Animator animator;
    private Rigidbody rb;
    public PuzzleGun puzzleGun;
    private bool destroying;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip destroySound;
    public CollisionSound collisionSound;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();

        if (!audioSource)
            audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void SpawnNewObject()
    {
        spawner.SpawnNewCube();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PhizDoor>())
        {
            if (spawner)
                spawner.SpawnNewCube();
            else
                DestroyObject();
        }

        if (other.GetComponent<PreasurePad>())
        {
            preasurePad = other.GetComponent<PreasurePad>();
        }
    }

    public void DestroyObject()
    {
        if (!destroying)
        {
            destroying = true;

            if (preasurePad && gameObject.GetComponent<BoxCollider>())
            {
                preasurePad.RemoveObject(gameObject.GetComponent<BoxCollider>());
            }

            if (animator)
            {
                gameObject.tag = "noCollide";

                animator.Play("CubeDissolve");

                if (rb)
                {
                    rb.useGravity = false;
                }

                StartCoroutine("Kill", 2f);

                if (puzzleGun && puzzleGun._grabbedObject == rb)
                {
                    puzzleGun._grabbedObject = null;
                }

                if (audioSource)
                {
                    if (gameObject.GetComponent<CollisionSound>())
                    {
                        gameObject.GetComponent<CollisionSound>().canCreateSounds = false;
                    }

                    if (destroySound)
                        audioSource.clip = destroySound;
                    if (collisionSound)
                        collisionSound.enabled = false;

                    audioSource.volume = 0.3f;
                    audioSource.minDistance = 1;
                    audioSource.maxDistance = 5;

                    audioSource.Play();
                }
            }
            else
                StartCoroutine("Kill", 0);
        }
    }

    IEnumerator Kill(float time)
    {
        yield return new WaitForSeconds(time / 2);

        if (gameObject.GetComponent<BoxCollider>())
            gameObject.GetComponent<BoxCollider>().enabled = false;

        yield return new WaitForSeconds(time);

        if (puzzleGun && puzzleGun._grabbedObject == rb)
        {
            puzzleGun._grabbedObject = null;
        }

        Destroy(gameObject);
    }
}
