using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipLoad : MonoBehaviour
{
    public PlayerShipHealth playerHealth;
    public ShipWeaponManager weaponsManager;
    public MeshRenderer defultShipRenderer;
    public GameObject[] ships;

    [Header("Ship Weapons")]
    public ShipWeapon[] ship0PWeapons;
    public ShipWeapon[] ship1PWeapons;
    public ShipWeapon[] ship2PWeapons;
    public ShipWeapon[] ship3PWeapons;

    public ShipWeapon ship0SecondaryWeapon;
    public ShipWeapon ship1SecondaryWeapon;
    public ShipWeapon ship2SecondaryWeapon;
    public ShipWeapon ship3SecondaryWeapon;

    void Start()
    {
        LoadShip();
    }

    void LoadShip()
    {
        PlayerData data = SaveSystem.LoadLevel(W8SaveData.savePath);
        weaponsManager.primaryWeapons = new List<ShipWeapon>();

        if (data.currentShip == 0)
        {
            defultShipRenderer.enabled = true;

            playerHealth.SetStartingHealth(50);

            for(int i = 0; i < ship0PWeapons.Length; i++)
            {
                weaponsManager.primaryWeapons.Add(ship0PWeapons[i]);
            }
            weaponsManager.secondaryWeapons[0] = ship0SecondaryWeapon;
        }
        else
        {
            defultShipRenderer.enabled = false;
        }

        int health = 100;

        for (int i = 1; i < 4; i++)
        {
            if (data.currentShip == i)
            {
                ships[i].SetActive(true);
                playerHealth.SetStartingHealth(health);
            }
            else
            {
                ships[i].SetActive(false);
                health += 100;
            }
        }

        switch (data.currentShip)
        {
            case 1:
                for (int i = 0; i < ship1PWeapons.Length; i++)
                {
                    weaponsManager.primaryWeapons.Add(ship1PWeapons[i]);
                }
                weaponsManager.secondaryWeapons[0] = ship1SecondaryWeapon;
                break;
            case 2:
                for (int i = 0; i < ship2PWeapons.Length; i++)
                {
                    weaponsManager.primaryWeapons.Add(ship2PWeapons[i]);
                }
                weaponsManager.secondaryWeapons[0] = ship2SecondaryWeapon;
                break;
            case 3:
                for (int i = 0; i < ship3PWeapons.Length; i++)
                {
                    weaponsManager.primaryWeapons.Add(ship3PWeapons[i]);
                }
                weaponsManager.secondaryWeapons[0] = ship3SecondaryWeapon;
                break;
        }
    }
}
