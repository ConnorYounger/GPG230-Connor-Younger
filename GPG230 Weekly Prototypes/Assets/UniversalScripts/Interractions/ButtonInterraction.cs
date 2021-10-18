using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInterraction : Interactions
{
    public ButtonInterraction(Interactable i)
    {
        interactable = i;
    }

    public override void Interact()
    {
        base.Interact();
    }

    public override void Interact(GameObject ob)
    {
        if (ob.GetComponent<CubeSpawnerButton>())
        {
            ob.GetComponent<CubeSpawnerButton>().ButtonPress();
        }
    }
}
