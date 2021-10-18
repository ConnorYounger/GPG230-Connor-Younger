using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Interactions interactions;

    [Header("Universal")]
    public WeaponManager weaponManager;

    [System.Serializable] public enum interactables { weaponBase, button };
    public interactables interactableType;

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
}