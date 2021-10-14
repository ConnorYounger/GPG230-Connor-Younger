using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponBase : MonoBehaviour
{
    #region variables
    public Weapon weapon;

    public Transform weaponShootPoint;

    [HideInInspector] public bool weaponCanFire;

    [Header("WeaponStats")]
    public float weaponDamge = 1;
    public int bulletsPerShot = 1;
    public float fireRate = 1;
    public float ammoCount = 10;
    public float reloadTime = 5;
    public float bulletSpread = 1;
    public float effectiveRange = 10;
    public float effectiveRangeDropOffMultiplier = 2;

    [Header("WeaponRefrences")]
    public GameObject lineRendererPrefab;
    public Transform playerCam;

    private bool customLineRenderer;
    private GameObject weaponCoreGameObject;

    public Animator weaponAnimator;
    public bool lootBoxWeapon;

    [Header("MuzzleFlash")]
    public ParticleSystem defultMuzzleFlash;
    public float flashTime = 0.2f;

    public TMP_Text ammoCounterText;

    public GameObject defultHitFx;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] fireSound;

    #endregion

    #region defultMethods
    void Start()
    {
        playerCam = GameObject.Find("FirstPersonCharacter").transform;
    }

    void Update()
    {
        if(weapon != null && weaponCanFire)
        {
            weapon.Tick();

            FireWeaponInput();
            WeaponAimInput();
            WeaponReloadInput();
        }
    }

    #endregion

    #region customMethods
    void SetWeaponType(string weaponType)
    {
        // Set the weapon to the correct weapon type
        switch (weaponType)
        {
            case "singleShot":
                weapon = new SingleShot(this);
                break;
            //case "assaltRifle":
            //    weapon = new AssaltRifle(this);
            //    break;
            //case "shotGun":
            //    weapon = new ShotGun(this);
            //    customLineRenderer = true;
            //    break;
            //case "projectileWeapon":
            //    weapon = new ProjectileWeapon(this);
            //    break;
        }
    }

    public void FireWeaponInput()
    {
        weapon.FireWeaponInput();
    }

    public void WeaponAimInput()
    {
        weapon.WeaponAimInput();
    }

    public void WeaponReloadInput()
    {
        weapon.WeaponReloadInput();
    }

    public void UpdateUIElements(float ammoCount)
    {
        int newAmmoCount = Mathf.RoundToInt(ammoCount);

        //Debug.Log("Bullets in clip = " + newAmmoCount.ToString());
        Debug.Log(ammoCounterText);

        if (ammoCounterText)
        {
            ammoCounterText.text = "Ammo: " + newAmmoCount.ToString();
        }
    }

    public void CreateLineRenderer(Vector3 hitPoint)
    {
        //if (weaponCore.useLineRender)
        //{
        //    GameObject lineRendererGO = Instantiate(lineRendererPrefab, weaponShootPoint.position, weaponShootPoint.rotation);
        //    LineRenderer lineRenderer = lineRendererGO.GetComponent<LineRenderer>();

        //    lineRenderer.SetPosition(0, weaponShootPoint.position);
        //    lineRenderer.SetPosition(1, hitPoint);

        //    Destroy(lineRendererGO, 0.5f);
        //}
    }

    void SetMuzzleFlash()
    {
        if (defultMuzzleFlash)
        {
            defultMuzzleFlash.transform.position = weaponShootPoint.transform.position;
            defultMuzzleFlash.transform.parent = weaponCoreGameObject.transform;
            //defultMuzzleFlash.transform.rotation = weaponShootPoint.transform.rotation;
        }
    }

    public void ShowMuzzleFlash()
    {
        //defultMuzzleFlash.gameObject.SetActive(true);
        defultMuzzleFlash.Play();

        StopCoroutine("HideMuzzleFlash");
        StartCoroutine("HideMuzzleFlash");
    }

    IEnumerator HideMuzzleFlash()
    {
        yield return new WaitForSeconds(flashTime);

        //defultMuzzleFlash.gameObject.SetActive(false);
        defultMuzzleFlash.Stop();
    }

    public void CreateHitFx(GameObject hitFx, RaycastHit hit)
    {
        GameObject fx = Instantiate(hitFx, hit.point, Quaternion.LookRotation(hit.normal));

        Destroy(fx, 1);
    }

    #endregion
}
