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

        if(interactable.isWinUI && GameObject.Find("Player") && GameObject.Find("Player").GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>())
        {
            Transform player = GameObject.Find("Player").transform;
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;

            if (player.GetChild(0) && player.GetChild(0).GetChild(0))
            {
                player.GetChild(0).GetChild(0).gameObject.SetActive(false);
            }

            if(interactable.week4Win.ToString() == "endStory1")
            {
                PlayerPrefs.SetFloat("endStory1", 0);
            }
            else
            {
                PlayerPrefs.SetFloat("endStory2", 0);
            }
        }
    }

    public override void Interact(GameObject ob)
    {
        Interact();
    }
}