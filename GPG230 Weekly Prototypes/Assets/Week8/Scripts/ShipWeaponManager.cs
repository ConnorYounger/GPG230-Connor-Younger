using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipWeaponManager : MonoBehaviour
{
    public ShipWeapon[] primaryWeapons;
    public ShipWeapon[] secondaryWeapons;

    public int secondaryAmmoCount = 10;

    void Start()
    {
        SetStartingWeaponStats();
    }

    void SetStartingWeaponStats()
    {
        //for(int i = 0; i < primaryWeaponSlots.Length; i++)
        //{
        //    ShipWeapon newWeapon = new ShipWeapon();

        //    newWeapon.projectilePrefab = primaryWeaponSlots[i].weapon.projectilePrefab;
        //    newWeapon.shootPoint = primaryWeaponSlots[i].shootPoint;
        //    newWeapon.fireRate = primaryWeaponSlots[i].weapon.fireRate;
        //    newWeapon.projectileSpeed = primaryWeaponSlots[i].weapon.projectileSpeed;
        //    newWeapon.projectileLifeTime = primaryWeaponSlots[i].weapon.projectileLifeTime;

        //    primaryWeapons.Add(newWeapon);
        //}

        //for (int i = 0; i < secondaryWeaponSlots.Length; i++)
        //{
        //    ShipWeapon newWeapon = new ShipWeapon();

        //    newWeapon.projectilePrefab = secondaryWeaponSlots[i].weapon.projectilePrefab;
        //    newWeapon.shootPoint = secondaryWeaponSlots[i].shootPoint;
        //    newWeapon.fireRate = secondaryWeaponSlots[i].weapon.fireRate;
        //    newWeapon.projectileSpeed = secondaryWeaponSlots[i].weapon.projectileSpeed;
        //    newWeapon.projectileLifeTime = secondaryWeaponSlots[i].weapon.projectileLifeTime;

        //    secondaryWeapons.Add(newWeapon);
        //}
    }

    void Update()
    {
        PlayerInput();
    }

    void PlayerInput()
    {
        if (Input.GetButton("Fire1"))
        {
            for (int i = 0; i < primaryWeapons.Length; i++) 
            {
                FirePrimaryWeapons(i);
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            for (int i = 0; i < secondaryWeapons.Length; i++)
            {
                FireSecondaryWeapon(i);
            }
        }
    }

    public void FirePrimaryWeapons(int i)
    {
        if (primaryWeapons[i].canFire)
        {
            FireProjectile(primaryWeapons[i]);
        }
    }

    public void FireSecondaryWeapon(int i)
    {
        if(secondaryAmmoCount > 0)
        {
            if (secondaryWeapons[i].canFire)
            {
                secondaryAmmoCount--;

                FireProjectile(secondaryWeapons[i]);
            }
        }
        else
        {
            // Play no ammo sound
            // Display no ammo text
        }
    }

    public void FireProjectile(ShipWeapon weapon)
    {
        weapon.canFire = false;
        //Debug.Log(weapon.canFire);

        GameObject projectile = Instantiate(weapon.weapon.projectilePrefab, weapon.shootPoint.position, weapon.shootPoint.rotation);
        ShipProjectile shipProjectile = projectile.GetComponent<ShipProjectile>();

        if (shipProjectile != null)
        {
            shipProjectile.projectileDamage = weapon.weapon.damage;
            shipProjectile.projectileSpeed = weapon.weapon.projectileSpeed;
        }

        projectile.layer = 14;

        Destroy(projectile, weapon.weapon.projectileLifeTime);

        StartCoroutine("WeaponCoolDown", weapon);
    }

    IEnumerator WeaponCoolDown(ShipWeapon weapon)
    {
        yield return new WaitForSeconds(weapon.weapon.fireRate);

        weapon.canFire = true;
    }
}
