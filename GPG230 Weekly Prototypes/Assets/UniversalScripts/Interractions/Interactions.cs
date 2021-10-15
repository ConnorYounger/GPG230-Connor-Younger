using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactions
{
    public Interactable interactable;

    public virtual void Interact()
    {

    }

    public virtual void Interact(GameObject ob)
    {

    }

    public virtual void DisableBoxCollider()
    {
        if(interactable.GetComponent<BoxCollider>())
            interactable.GetComponent<BoxCollider>().enabled = false;
    }
}
