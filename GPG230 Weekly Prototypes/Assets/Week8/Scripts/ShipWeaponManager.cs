using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipWeaponManager : MonoBehaviour
{
    [System.Serializable]
    public struct weaponSlots 
    {
        public ShipWeaponStats weapon;
        public Transform shootPoint;
        public bool canFire;
    }

    public weaponSlots[] primaryWeaponSlots;
    public weaponSlots[] secondaryWeaponSlots;

    public int secondaryAmmoCount = 10;

    void Start()
    {
        SetStartingWeaponStats();
    }

    void SetStartingWeaponStats()
    {
        for(int i = 0; i < primaryWeaponSlots.Length; i++)
        {
            primaryWeaponSlots[i].canFire = true;
        }

        for (int i = 0; i < secondaryWeaponSlots.Length; i++)
        {
            secondaryWeaponSlots[i].canFire = true;
        }
    }

    void Update()
    {
        PlayerInput();
    }

    void PlayerInput()
    {
        if (Input.GetButton("Fire1"))
        {
            for (int i = 0; i < primaryWeaponSlots.Length; i++) 
            {
                FirePrimaryWeapons(i);
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            for (int i = 0; i < secondaryWeaponSlots.Length; i++)
            {
                FireSecondaryWeapon(i);
            }
        }
    }

    public void FirePrimaryWeapons(int i)
    {
        if (primaryWeaponSlots[i].canFire)
        {
            FireProjectile(primaryWeaponSlots[i]);
        }
    }

    public void FireSecondaryWeapon(int i)
    {
        if(secondaryAmmoCount > 0)
        {
            if (secondaryWeaponSlots[i].canFire)
            {
                secondaryAmmoCount--;

                FireProjectile(secondaryWeaponSlots[i]);
            }
        }
        else
        {
            // Play no ammo sound
            // Display no ammo text
        }
    }

    public void FireProjectile(weaponSlots weapon)
    {
        weapon.canFire = false;

        GameObject projectile = Instantiate(weapon.weapon.projectilePrefab, weapon.shootPoint.position, weapon.shootPoint.rotation);
        ShipProjectile shipProjectile = projectile.GetComponent<ShipProjectile>();

        if (shipProjectile != null)
        {
            shipProjectile.projectileSpeed = weapon.weapon.projectileSpeed;
        }

        Destroy(projectile, weapon.weapon.projectileLifeTime);

        StartCoroutine("WeaponCoolDown", weapon);
    }

    IEnumerator WeaponCoolDown(weaponSlots weapon)
    {
        yield return new WaitForSeconds(weapon.weapon.fireRate);

        //weapon.canFire = true;
    }
}
