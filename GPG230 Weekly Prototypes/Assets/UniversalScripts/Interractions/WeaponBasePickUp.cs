using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBasePickUp : Interactions
{
    public WeaponBasePickUp(Interactable i)
    {
        interactable = i;
    }

    public override void Interact()
    {
        base.Interact();
    }

    public override void Interact(GameObject ob)
    {
        interactable.weaponManager.PickUpWeapon(ob);

        if (ob.GetComponent<PhysicsGun>())
        {
            ob.GetComponent<PhysicsGun>().canUse = true;
        }
        else if (ob.GetComponent<ObjectSpawnerGun>())
        {
            ob.GetComponent<ObjectSpawnerGun>().canUse = true;
        }

        DisableBoxCollider();
    }
}
