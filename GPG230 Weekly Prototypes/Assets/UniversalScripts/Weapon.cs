using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon
{
    // Stats
    public float bulletDamage;
    public int bulletsPerShot;
    public float fireRate;
    public float ammoCount;
    public float reloadTime;
    public float bulletSpread;
    public float effectiveRange;
    public float effectiveRangeDropOffMultiplier;

    public float bulletsInClip;
    public float fireRateTimer;
    public float reloadTimer;

    public bool canFire = true;

    public WeaponBase weaponBase;

    public bool fireAudioIsPlaying;

    public virtual void Tick()
    {
        FireRateTimer();
        ReloadTimer();
    }

    public virtual void FireRateTimer()
    {
        if (fireRateTimer > 0)
            fireRateTimer -= Time.deltaTime;
    }

    public virtual void ReloadTimer()
    {
        if (!canFire)
        {
            if (reloadTimer > 0)
            {
                reloadTimer -= Time.deltaTime;
                weaponBase.weaponAnimator.SetBool("isReloading", true);
            }
            else
            {
                //Debug.Log("Reload Completed");

                bulletsInClip = ammoCount;
                weaponBase.UpdateUIElements(bulletsInClip);
                weaponBase.weaponAnimator.SetBool("isReloading", false);
                weaponBase.weaponAnimator.speed = 1;
                canFire = true;
            }
        }
    }

    public virtual void FireWeaponInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            FireWeapon();
        }
    }

    public virtual void WeaponAimInput()
    {
        //if (Input.GetButtonDown("Fire2"))
        //{
        //    weaponBase.weaponAnimator.SetBool("isAiming", true);
        //}

        //if (Input.GetButtonUp("Fire2"))
        //{
        //    weaponBase.weaponAnimator.SetBool("isAiming", false);
        //}

        if (Input.GetButton("Fire2"))
        {
            weaponBase.weaponAnimator.SetBool("isAiming", true);
        }
        else
        {
            weaponBase.weaponAnimator.SetBool("isAiming", false);
        }
    }

    public virtual void WeaponReloadInput()
    {
        if (Input.GetKeyDown(KeyCode.R) && bulletsInClip < ammoCount)
        {
            Reload();
        }
    }

    public virtual void FireWeapon()
    {
        //Debug.Log("Fire weapon atempt");

        if (bulletsInClip > 0 && canFire)
        {
            if(fireRateTimer <= 0)
            {
                if (weaponBase.weaponAnimator)
                {
                    if (weaponBase.weaponAnimator.GetBool("isAiming"))
                    {
                        weaponBase.weaponAnimator.Rebind();
                        weaponBase.weaponAnimator.Play("FireAimedWeapon");
                    }
                    else
                    {
                        weaponBase.weaponAnimator.Rebind();
                        weaponBase.weaponAnimator.Play("FireWeapon");
                    }
                }

                bulletsInClip--;

                for (int i = 0; i < bulletsPerShot; i++)
                    FireBullet();

                if (weaponBase.audioSource && !fireAudioIsPlaying)
                {
                    if(weaponBase.fireSound.Length > 0)
                    {
                        int rand = Random.Range(0, weaponBase.fireSound.Length);
                        weaponBase.audioSource.clip = weaponBase.fireSound[rand];
                        weaponBase.audioSource.Play();
                    }
                }
            }
        }
        else
        {
            //Debug.Log("Can not fire weapon");
            // Play empty clip sound
        }
    }

    public virtual void UseLineRenderer(Vector3 hitPoint)
    {
        if (weaponBase.lineRendererPrefab)
        {
            weaponBase.CreateLineRenderer(hitPoint);
        }
    }

    public virtual void FireBullet()
    {
        fireRateTimer = fireRate;
        //Debug.Log("Bullets in clip = " + bulletsInClip);
        weaponBase.UpdateUIElements(bulletsInClip);

        AmmoCheck();

        if(bulletSpread < 0)
        {
            bulletSpread = 0.0001f;
        }

        float randBloomX = Random.Range(-bulletSpread, bulletSpread);
        float randBloomY = Random.Range(-bulletSpread, bulletSpread);
        float randBloomZ = Random.Range(-bulletSpread, bulletSpread);
        //float randBloomZ = Random.Range(-bulletSpread, bulletSpread);
        Vector3 newBloom = new Vector3(weaponBase.playerCam.forward.x + randBloomX, weaponBase.playerCam.forward.y + randBloomY, weaponBase.playerCam.forward.z + randBloomZ);

        RaycastHit hit;
        Physics.Raycast(weaponBase.playerCam.position, newBloom, out hit, 1000, 7);

        if (hit.collider != null)
        {
            if (hit.collider.GetComponent<EnemyHealth>())
            {
                DealDamage(hit.collider.GetComponent<EnemyHealth>());

                if (hit.collider.GetComponent<EnemyHealth>().defultHitFx)
                {
                    weaponBase.CreateHitFx(hit.collider.GetComponent<EnemyHealth>().defultHitFx, hit);
                }
            }
            else if (hit.collider.GetComponent<EnemyHead>())
            {
                DealHeadshotDamage(hit.collider.GetComponent<EnemyHead>());

                if (hit.collider.GetComponent<EnemyHead>().enemyHealth.specialHitFx)
                {
                    weaponBase.CreateHitFx(hit.collider.GetComponent<EnemyHead>().enemyHealth.specialHitFx, hit);
                }
            }
            else
            {
                if (weaponBase.defultHitFx)
                {
                    weaponBase.CreateHitFx(weaponBase.defultHitFx, hit);
                }
            }

            UseLineRenderer(hit.point);

            // Create Particle effects
            Debug.DrawLine(weaponBase.playerCam.position, hit.point, Color.red);

            //Debug.Log("Fire Weapon: Target Found, " + hit.collider.name);
        }
        else
        {
            //Debug.Log("Fire Weapon: No Target Hit");
        }

        if (weaponBase.defultMuzzleFlash)
        {
            //weaponBase.defultMuzzleFlash.Play();
            weaponBase.ShowMuzzleFlash();
        }
    }

    public virtual void AmmoCheck()
    {
        if(bulletsInClip <= 0 && canFire)
        {
            Reload();
        }
    }

    public virtual void Reload()
    {
        if (weaponBase.ammoCounterText)
        {
            weaponBase.ammoCounterText.text = "--";
        }

        float newSpeed = 2 / reloadTime;
        weaponBase.weaponAnimator.speed = newSpeed;

        reloadTimer = reloadTime;
        canFire = false;
    }

    public virtual void DealDamage(EnemyHealth enemy)
    {
        //Debug.Log("Deal: " + calculatedDamage + " damage");

        enemy.TakeDamage(CalculateDamage(enemy.transform.position));
    }

    public virtual void DealHeadshotDamage(EnemyHead enemy)
    {
        enemy.DealHeadShotDamage(CalculateDamage(enemy.transform.position));
    }

    public virtual float CalculateDamage(Vector3 pos)
    {
        float calculatedDamage = bulletDamage;
        float distance = Vector3.Distance(weaponBase.playerCam.position, pos);

        if (distance > effectiveRange)
        {
            calculatedDamage -= (distance - effectiveRange) * effectiveRangeDropOffMultiplier;
        }

        if (calculatedDamage < 1)
        {
            calculatedDamage = 1;
        }

        return calculatedDamage;
    }

    public void SetWeaponStats(float d, int bps, float fr, float ac, float rt, float bs, float er, float erm)
    {
        bulletDamage = d;
        bulletsPerShot = bps;
        fireRate = fr;
        ammoCount = ac;
        reloadTime = rt;
        bulletSpread = bs;
        effectiveRange = er;
        effectiveRangeDropOffMultiplier = erm;

        bulletsInClip = ammoCount;
        canFire = true;

        //Debug.Log("Set weapon stats");
    }
}
