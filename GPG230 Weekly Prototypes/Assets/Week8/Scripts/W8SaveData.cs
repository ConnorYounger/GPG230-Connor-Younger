using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class W8SaveData : MonoBehaviour
{
    public static int playerScore;
    public static string savePath = "/W8SaveData";

    public int currentShip;
    public ShipSaveData[] shipSaveData;
    public bool[] shipsUnlocked;

    // Start is called before the first frame update
    void Start()
    {
        //SaveSystem.SaveStats();
        //ResetSaveStats();

        LoadSave();
    }

    public void ResetSaveStats()
    {
        playerScore = 0;
        currentShip = 0;

        SaveSystem.SaveStats(this);

        PlayerData data = SaveSystem.LoadLevel(savePath);

        data.currentShip = 0;
        //data.shipsUnlocked = new bool[4];
        //data.shipsUnlocked[0] = true;
        //data.shipHull = new int[4];
        //data.primaryWeaponLevel = new int[4];
        //data.secondaryWeaponLevel = new int[4];
        //data.primaryWeaponName = new string[4];
        //data.secondaryWeaponName = new string[4];

        SaveSystem.SaveStats(this);
    }

    void LoadSave()
    {
        PlayerData data = SaveSystem.LoadLevel(savePath);

        currentShip = data.currentShip;
        //shipSaveData = data.shipSaveData;
        shipsUnlocked = data.shipsUnlocked;

        for(int i = 0; i < shipSaveData.Length; i++)
        {
            shipSaveData[i].shipHull = data.shipHull[i];
            shipSaveData[i].primaryWeapon.weaponName = data.primaryWeaponName[i];
            shipSaveData[i].primaryWeapon.weaponLevel = data.primaryWeaponLevel[i];
            shipSaveData[i].secondaryWeapon.weaponName = data.secondaryWeaponName[i];
            shipSaveData[i].secondaryWeapon.weaponLevel = data.secondaryWeaponLevel[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            AddCurrency(100);
        }
    }

    public static void AddCurrency(int amount)
    {
        PlayerData data = SaveSystem.LoadLevel(W8SaveData.savePath);
        playerScore = data.w8PlayerCurrency;

        // Save level stats
        if (data != null)
        {
            playerScore += amount;
            //Debug.Log("Add: " + amount.ToString() + ", Total: " + data.w8PlayerCurrency);

            SaveSystem.SaveStats();
        }
        else
        {
            Debug.LogError("Could not load " + data);
        }
    }

    public void NewSave()
    {
        
    }
}
