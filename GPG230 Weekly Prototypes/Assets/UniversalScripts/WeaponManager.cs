using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponManager : MonoBehaviour
{
    //public GameManager gameManager;

    public Transform weaponDisplayPos;

    public GameObject startingWeaponPrefab;
    public List<GameObject> heldWeapons;
    public int maxHeldWeapons = 2;
    public int currentWeaponIndex;

    [Header("UI Elements")]
    public TMP_Text ammoCounter;

    // Start is called before the first frame update
    void Start()
    {
        heldWeapons = new List<GameObject>();

        if(startingWeaponPrefab)
            SetStartingWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        SwitchWeaponInput();
    }

    void SetStartingWeapon()
    {
        if (startingWeaponPrefab.GetComponent<WeaponBase>())
        {
            //GameObject spawnedStartingWeapon = gameManager.lootBoxManager.SpawnSetWeapon(startingWeaponPrefab.GetComponent<WeaponBase>(), transform);
            //PickUpWeapon(startingWeaponPrefab);
            PickUpWeapon(startingWeaponPrefab);
        }
    }

    void SwitchWeaponInput()
    {
        if (heldWeapons.Count > 0 && !Input.GetKey(KeyCode.Mouse1))
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (currentWeaponIndex < heldWeapons.Count - 1)
                {
                    currentWeaponIndex++;
                }
                else
                {
                    currentWeaponIndex = 0;
                }

                SwitchWeapons();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (currentWeaponIndex > 0)
                {
                    currentWeaponIndex--;
                }
                else
                {
                    currentWeaponIndex = heldWeapons.Count - 1;
                }

                SwitchWeapons();
            }
        }
    }

    public void PickUpWeapon(GameObject weapon)
    {
        if (heldWeapons.Count > 0 && heldWeapons.Count >= maxHeldWeapons)
        {
            GameObject weaponToDespawn = heldWeapons[currentWeaponIndex];
            heldWeapons.Remove(weaponToDespawn);
            Destroy(weaponToDespawn);
        }

        heldWeapons.Add(weapon);
        weapon.transform.parent = weaponDisplayPos.transform;
        weapon.transform.position = weaponDisplayPos.position;
        weapon.transform.rotation = weaponDisplayPos.rotation;

        if (weapon.GetComponent<WeaponBase>())
        {
            WeaponBase weaponBase = weapon.GetComponent<WeaponBase>();

            if (!weaponBase.ammoCounterText)
            {
                weaponBase.ammoCounterText = ammoCounter;
            }
        }

        currentWeaponIndex = heldWeapons.IndexOf(weapon);
        weapon.SetActive(false);

        SwitchWeapons();
    }

    public void SwitchWeapons()
    {
        for (int i = 0; i < heldWeapons.Count; i++)
        {
            if (i == currentWeaponIndex)
            {
                heldWeapons[i].SetActive(true);
                heldWeapons[i].GetComponent<WeaponBase>().weaponCanFire = true;

                if(heldWeapons[i].GetComponent<WeaponBase>().ammoCounterText)
                    heldWeapons[i].GetComponent<WeaponBase>().ammoCounterText.text = heldWeapons[i].GetComponent<WeaponBase>().ammoCount.ToString();

                if (heldWeapons[i].GetComponent<WeaponBase>().weapon != null)
                {
                    heldWeapons[i].GetComponent<WeaponBase>().ammoCounterText.text = heldWeapons[i].GetComponent<WeaponBase>().weapon.bulletsInClip.ToString();
                }

                if (heldWeapons[i].GetComponent<WeaponBase>().weapon != null && heldWeapons[i].GetComponent<WeaponBase>().weapon.reloadTimer > 0)
                {
                    heldWeapons[i].GetComponent<WeaponBase>().weapon.Reload();
                }
            }
            else
            {
                heldWeapons[i].GetComponent<WeaponBase>().weaponCanFire = false;
                heldWeapons[i].SetActive(false);
            }
        }
    }
}
