using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Interactions interactions;

    [System.Serializable] public enum interactables { weaponBase, button, uiInteraction };
    public interactables interactableType;

    [Header("Universal")]
    public WeaponManager weaponManager;
    public GameObject hoverOverUI;

    [Header("Non Universal")]
    public GameObject openUI;
    public bool isWinUI = true;

    // Start is called before the first frame update
    void Start()
    {
        SetInteractable();

        weaponManager = GameObject.Find("Player").GetComponent<WeaponManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetInteractable()
    {
        switch (interactableType.ToString()) 
        {
            case "weaponBase":
                interactions = new WeaponBasePickUp(this);
                break;
            case "button":
                interactions = new ButtonInterraction(this);
                break;
            case "uiInteraction":
                interactions = new UIInteraction(this);
                break;
        }
    }

    public void Interract()
    {
        interactions.Interact();
    }

    public void Interract(GameObject o)
    {
        interactions.Interact(o);
    }

    public void InteractionOver()
    {
        if (hoverOverUI)
        {
            hoverOverUI.SetActive(true);

            StopCoroutine("HideInteractionOver");
            StartCoroutine("HideInteractionOver");
        }
    }

    IEnumerator HideInteractionOver()
    {
        yield return new WaitForSeconds(0.1f);

        if(hoverOverUI)
            hoverOverUI.SetActive(false);
    }
}