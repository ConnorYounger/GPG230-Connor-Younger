using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInteraction : Interactions
{
    public UIInteraction(Interactable i)
    {
        interactable = i;
    }

    public override void Interact()
    {
        interactable.openUI.SetActive(true);
        Time.timeScale = 0;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if(interactable.isWinUI && GameObject.Find("Player"))
        {
            GameObject.Find("Player").SetActive(false);
        }
    }

    public override void Interact(GameObject ob)
    {
        Interact();
    }
}