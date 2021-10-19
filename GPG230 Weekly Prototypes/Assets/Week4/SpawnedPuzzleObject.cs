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

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
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

            StartCoroutine("Kill", 1.5f);

            if (puzzleGun && puzzleGun._grabbedObject == rb)
            {
                puzzleGun._grabbedObject = null;
            }
        }
        else
            StartCoroutine("Kill", 0);
    }

    IEnumerator Kill(float time)
    {
        yield return new WaitForSeconds(time);

        if (puzzleGun && puzzleGun._grabbedObject == rb)
        {
            puzzleGun._grabbedObject = null;
        }

        Destroy(gameObject);
    }
}
