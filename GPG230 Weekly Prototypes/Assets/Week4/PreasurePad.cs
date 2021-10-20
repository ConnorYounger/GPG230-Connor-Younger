using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreasurePad : MonoBehaviour
{
    [System.Serializable] public enum buttonTypes { defult, green, blue, yellow };
    public buttonTypes buttonType;

    public PuzzleConnections connector;
    public PuzzleConnections[] extraConnector;
    public MeshRenderer mesh;
    public Animator animator;

    public bool singlePress;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] turnOnSounds;
    public AudioClip[] turnOffSounds;

    private List<Collider> colliders;

    // Start is called before the first frame update
    void Start()
    {
        colliders = new List<Collider>();
        connector = transform.parent.GetComponent<PuzzleConnections>();

        if(extraConnector.Length > 0)
            AddExtraConnections();
    }

    // Update is called once per frame
    void Update()
    {
        //CheckForCollisions();
    }

    void AddExtraConnections()
    {
        foreach(PuzzleConnections p in extraConnector)
        {
            p.AddConnection(transform);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        AddCollision(collision.collider);
    }

    void AddCollision(Collider collision)
    {
        if(buttonType == buttonTypes.defult && collision.tag != "noCollide")
        {
            colliders.Add(collision);
        }
        else
        {
            switch (collision.gameObject.tag.ToString())
            {
                case "green":
                    if(buttonType == buttonTypes.green)
                        colliders.Add(collision);
                    break;
                case "blue":
                    if (buttonType == buttonTypes.blue)
                        colliders.Add(collision);
                    break;
                case "yellow":
                    if (buttonType == buttonTypes.yellow)
                        colliders.Add(collision);
                    break;
            }
        }
        
        CheckForCollisions();
    }

    private void OnCollisionExit(Collision collision)
    {
        colliders.Remove(collision.collider);

        CheckForCollisions();
    }

    private void OnTriggerEnter(Collider other)
    {
        AddCollision(other);
    }

    private void OnTriggerExit(Collider other)
    {
        RemoveObject(other);
    }

    public void RemoveObject(Collider other)
    {
        colliders.Remove(other);

        if (connector.phisDoor)
        {
            connector.phisDoor.TurnOff(connector.gameObject);
        }

        CheckForCollisions();
    }

    void CheckForCollisions()
    {
        //Debug.Log("CheckForCollisions");

        if(colliders.Count > 0)
        {
            connector.Activate(transform);
            //Debug.Log("Activate");

            if (extraConnector.Length > 0)
            {
                foreach(PuzzleConnections pz in extraConnector)
                {
                    pz.Activate(transform);
                }
            }

            if (!animator.GetBool("PresurePadDown"))
            {
                animator.SetBool("PresurePadDown", true);
                animator.SetBool("PresurePadUp", false);

                if (audioSource && turnOnSounds.Length > 0)
                {
                    int rand = Random.Range(0, turnOnSounds.Length);
                    audioSource.clip = turnOnSounds[rand];
                    audioSource.Play();
                }
            }
        }
        else
        {
            if (!singlePress)
            {
                connector.Deactivate(transform);
                //Debug.Log("Deactivate");

                if (extraConnector.Length > 0)
                {
                    foreach (PuzzleConnections pz in extraConnector)
                    {
                        pz.Deactivate(transform);
                    }
                }

                if (!animator.GetBool("PresurePadUp"))
                {
                    animator.SetBool("PresurePadUp", true);
                    animator.SetBool("PresurePadDown", false);

                    if (audioSource && turnOffSounds.Length > 0)
                    {
                        int rand = Random.Range(0, turnOffSounds.Length);
                        audioSource.clip = turnOffSounds[rand];
                        audioSource.Play();
                    }
                }
            }
        }
    }
}
