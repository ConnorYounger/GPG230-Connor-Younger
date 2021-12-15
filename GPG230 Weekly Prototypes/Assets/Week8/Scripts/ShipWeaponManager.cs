using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ShipWeaponManager : MonoBehaviourPunCallbacks /*, IPunObservable */
{
    //#region IPunObservable implementation

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (photonView)
    //    {
    //        if (stream.IsWriting)
    //        {
    //            // We own this player: send the others our data
    //            //stream.SendNext(transformRotationX);

    //            stream.SendNext(weaponDirX);
    //            stream.SendNext(weaponDirY);
    //            stream.SendNext(weaponDirZ);
    //        }
    //        else
    //        {
    //            // Network player, receive data
    //            //this.transformRotationX = (float)stream.ReceiveNext();

    //            this.weaponDirX = (float)stream.ReceiveNext();
    //            this.weaponDirY = (float)stream.ReceiveNext();
    //            this.weaponDirZ = (float)stream.ReceiveNext();
    //        }
    //    }
    //}

    //#endregion

    public List<ShipWeapon> primaryWeapons;
    public float weaponDirX;
    public float weaponDirY;
    public float weaponDirZ;
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

    public PhotonView photonView;
    public PhotonPlayerManager photonPlayerManager;

    private ShipWeapon currentShipWeapon;
    private ShipProjectile currentProjectile;
    public List<ShipProjectile> spawnedProjectiles;

    void Start()
    {
        photonView = gameObject.GetComponent<PhotonView>();
        photonPlayerManager = gameObject.GetComponent<PhotonPlayerManager>();


        if (photonView)
        {
            //weaponDirX = new List<float>();
            //weaponDirY = new List<float>();
            //weaponDirZ = new List<float>();
            //weaponDirW = new List<float>();
        }

        SetStartingWeaponStats();

        if (photonView)
        {
            spawnedProjectiles = new List<ShipProjectile>();
        }
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
        if (photonView == null)
        {
            PlayerInput();
            CoolDownTimer();
        }
        else
        {
            if (photonView.IsMine)
            {
                PlayerInput();
                CoolDownTimer();
            }
        }
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
                    //FirePrimaryWeapons(i);
                    photonView.RPC("FirePrimaryWeapons", RpcTarget.All, i);
                }
            }
        }
    }

    [PunRPC]
    public void FirePrimaryWeapons(int i)
    {
        if (primaryWeapons[i].canFire)
        {
            if (photonView == null)
            {
                //FireProjectile(primaryWeapons[i]);
                currentShipWeapon = primaryWeapons[i];
                FireProjectile();
            }
            else
            {
                currentShipWeapon = primaryWeapons[i];
                //if (photonView.IsMine)
                //{
                //    weaponDir = currentShipWeapon.lookPoint;
                //}
                //else
                //{
                //    currentShipWeapon.lookPoint = weaponDir;
                //    currentShipWeapon.transform.LookAt(currentShipWeapon.lookPoint);
                //}
                //photonView.RPC("FireProjectile", RpcTarget.AllBuffered);
                FireProjectile();
            }

            if (audioSource && primaryFireSounds.Length > 0)
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

                //FireProjectile(secondaryWeapons[i]);
                currentShipWeapon = secondaryWeapons[i];
                FireProjectile();

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

    [PunRPC]
    public void FireProjectile()
    {
        ShipWeapon weapon = currentShipWeapon;
        //Debug.Log(weapon.canFire);

        GameObject projectile = new GameObject();

        if(photonView == null)
        {
            projectile = Instantiate(weapon.weapon.projectilePrefab, weapon.shootPoint.position, weapon.shootPoint.rotation);
        }
        else
        {
            //projectile = PhotonNetwork.Instantiate(weapon.weapon.projectilePrefab.name, weapon.shootPoint.position, weapon.shootPoint.rotation);
            projectile = Instantiate(weapon.weapon.projectilePrefab, weapon.shootPoint.position, weapon.shootPoint.rotation);

            //if (photonView.IsMine)
            //{
            //    //weaponDirX = currentShipWeapon.gameObject.transform.rotation.x;
            //    //weaponDirX = currentShipWeapon.gameObject.transform.rotation.y;
            //    //weaponDirX = currentShipWeapon.gameObject.transform.rotation.z;
            //    //weaponDirX = currentShipWeapon.gameObject.transform.rotation.w;
            //    weaponDir = currentShipWeapon.lookPoint;
            //}
            //else
            //{
            //    //Quaternion newRot = new Quaternion(weaponDirX, weaponDirY, weaponDirZ, weaponDirW);
            //    //currentShipWeapon.gameObject.transform.rotation = newRot;
            //    currentShipWeapon.lookPoint = weaponDir;
            //    currentShipWeapon.transform.LookAt(currentShipWeapon.lookPoint);
            //}
        }

        weapon.canFire = false;

        ShipProjectile shipProjectile = projectile.GetComponent<ShipProjectile>();
        spawnedProjectiles.Add(shipProjectile);
        shipProjectile.shipWeaponManager = this;

        if (photonView == null)
        {
            projectile.layer = 14;

            if (weapon.shootFx)
            {
                weapon.shootFx.SetActive(true);
                weapon.StopCoroutine("FireParticleFx");
                weapon.StartCoroutine("FireParticleFx");
            }
        }
        else
        {
            shipProjectile.photonView = photonView;

            if (photonView.IsMine)
            {
                weapon.shootFx.SetActive(true);
                weapon.StopCoroutine("FireParticleFx");
                weapon.StartCoroutine("FireParticleFx");
            }
        }

        if (shipProjectile != null)
        {
            shipProjectile.projectileDamage = weapon.weapon.damage;
            shipProjectile.projectileSpeed = weapon.weapon.projectileSpeed;
            shipProjectile.lifeTime = weapon.weapon.projectileLifeTime;
            shipProjectile.StartCoroutine("ProjectileLifeTime");
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

    public void DestroySpecificProjectile(ShipProjectile projectile)
    {
        if (photonView.IsMine)
        {
            currentProjectile = projectile;

            photonView.RPC("DestroyProjectile", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void DestroyProjectile()
    {
        if(currentProjectile != null && spawnedProjectiles.Count > 0)
        {
            ShipProjectile projectileToDestroy = null;

            foreach(ShipProjectile spawnedProjectile in spawnedProjectiles)
            {
                if(spawnedProjectile == currentProjectile)
                {
                    projectileToDestroy = spawnedProjectile;
                }
            }

            if (projectileToDestroy != null)
            {
                spawnedProjectiles.Remove(projectileToDestroy);
                projectileToDestroy.DestroyProjectile();
            }
        }
    }

    //[PunRPC]
    //public void DestroyProjectile(ShipProjectile projectile)
    //{
    //    if (projectile.destroyEffect)
    //    {
    //        if (photonView == null)
    //        {
    //            GameObject fx = Instantiate(projectile.destroyEffect, projectile.transform.position, projectile.transform.rotation);
    //            Destroy(fx, 3);
    //        }
    //        else
    //        {
    //            if (photonView.IsMine && projectile.spawnMultiDestroyEffect)
    //            {
    //                PhotonNetwork.Instantiate(projectile.destroyEffectMulti.name, projectile.transform.position, projectile.transform.rotation);
    //            }
    //            else
    //            {
    //                GameObject fx = Instantiate(projectile.destroyEffect, projectile.transform.position, projectile.transform.rotation);
    //                Destroy(fx, 3);
    //            }
    //        }
    //    }

    //    projectile.StopCoroutine("ProjectileLifeTime");

    //    if (photonView == null)
    //        Destroy(projectile.gameObject);
    //    else
    //        PhotonNetwork.Destroy(projectile.gameObject);
    //}
}
