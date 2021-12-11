using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ShipWeaponManager : MonoBehaviour
{
    public List<ShipWeapon> primaryWeapons;
    public ShipWeapon[] secondaryWeapons;

    public int secondaryAmmoCount = 10;

    public Image secondaryCoolDownImage;
    private float coolDownTime;
    private float coolDownTimer;

    public Color rocketReloadedColour;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] primaryFireSounds;
    public AudioClip[] secondaryFireSounds;

    public AudioSource audioSource2;
    public AudioClip rocketReloadSound;

    private PhotonView photonView;

    void Start()
    {
        photonView = gameObject.GetComponent<PhotonView>();

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
        CoolDownTimer();
    }

    void CoolDownTimer()
    {
        if (secondaryCoolDownImage && coolDownTimer < coolDownTime)
        {
            coolDownTimer += Time.deltaTime;

            secondaryCoolDownImage.fillAmount = (coolDownTimer / coolDownTime);
        }
        else
        {
            if (secondaryCoolDownImage)
            {
                secondaryCoolDownImage.color = rocketReloadedColour;
            }
        }
    }

    void PlayerInput()
    {
        if (photonView == null)
        {
            if (Input.GetButton("Fire1"))
            {
                for (int i = 0; i < primaryWeapons.Count; i++)
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
        else
        {
            if (photonView.IsMine && Input.GetButton("Fire1"))
            {
                for (int i = 0; i < primaryWeapons.Count; i++)
                {
                    FirePrimaryWeapons(i);
                }
            }
        }
    }

    public void FirePrimaryWeapons(int i)
    {
        if (primaryWeapons[i].canFire)
        {
            FireProjectile(primaryWeapons[i]);

            if(audioSource && primaryFireSounds.Length > 0)
            {
                int rand = Random.Range(0, primaryFireSounds.Length);
                audioSource.clip = primaryFireSounds[rand];
                audioSource.Play();
            }
        }
    }

    public void FireSecondaryWeapon(int i)
    {
        if(secondaryAmmoCount > 0)
        {
            if (secondaryWeapons[i].canFire)
            {
                //secondaryAmmoCount--;

                FireProjectile(secondaryWeapons[i]);

                if (audioSource && secondaryFireSounds.Length > 0)
                {
                    int rand = Random.Range(0, secondaryFireSounds.Length);
                    audioSource.clip = secondaryFireSounds[rand];
                    audioSource.Play();
                }

                if (secondaryCoolDownImage)
                {
                    coolDownTime = secondaryWeapons[i].weapon.fireRate;
                    coolDownTimer = 0;

                    secondaryCoolDownImage.color = Color.white;
                }
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

        GameObject projectile = new GameObject();

        if(photonView == null)
        {
            projectile = Instantiate(weapon.weapon.projectilePrefab, weapon.shootPoint.position, weapon.shootPoint.rotation);
        }
        else
        {
            projectile = PhotonNetwork.Instantiate(weapon.weapon.projectilePrefab.name, weapon.shootPoint.position, weapon.shootPoint.rotation);
        }

        ShipProjectile shipProjectile = projectile.GetComponent<ShipProjectile>();

        if (photonView == null)
            projectile.layer = 14;
        else
        {
            shipProjectile.shipPhotonView = photonView;
        }

        if (shipProjectile != null)
        {
            shipProjectile.projectileDamage = weapon.weapon.damage;
            shipProjectile.projectileSpeed = weapon.weapon.projectileSpeed;
            shipProjectile.lifeTime = weapon.weapon.projectileLifeTime;
            shipProjectile.StartCoroutine("ProjectileLifeTime");
        }

        if (weapon.shootFx)
        {
            weapon.shootFx.SetActive(true);
            weapon.StopCoroutine("FireParticleFx");
            weapon.StartCoroutine("FireParticleFx");
        }

        //if(photonView == null)
        //    Destroy(projectile, weapon.weapon.projectileLifeTime);

        StartCoroutine("WeaponCoolDown", weapon);
    }

    IEnumerator WeaponCoolDown(ShipWeapon weapon)
    {
        yield return new WaitForSeconds(weapon.weapon.fireRate);

        if(audioSource2 && rocketReloadSound && weapon.weapon.name == "HomingMissileLauncher")
        {
            audioSource2.clip = rocketReloadSound;
            audioSource2.Play();
        }        

        weapon.canFire = true;
    }
}
