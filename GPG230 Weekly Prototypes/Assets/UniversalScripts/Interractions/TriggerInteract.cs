using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerInteract : MonoBehaviour
{
    public Interactable interactable;
    public GameObject interactObject;
    public AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        if (interactable)
        {
            if (interactObject)
                interactable.Interract(interactObject);
            else
                interactable.Interract();

            if (audioSource)
                audioSource.Play();

            if (gameObject.GetComponent<BoxCollider>())
                gameObject.GetComponent<BoxCollider>().enabled = false;
            else
                gameObject.SetActive(false);
        }
    }
}
