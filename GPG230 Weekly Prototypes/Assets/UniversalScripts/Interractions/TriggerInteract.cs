using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerInteract : MonoBehaviour
{
    public Interactable interactable;
    public GameObject interactObject;

    private void OnTriggerEnter(Collider other)
    {
        if (interactable)
        {
            if (interactObject)
                interactable.Interract(interactObject);
            else
                interactable.Interract();

            gameObject.SetActive(false);
        }
    }
}
