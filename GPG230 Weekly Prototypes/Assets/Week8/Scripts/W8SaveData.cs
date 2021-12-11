using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class W8SaveData : MonoBehaviour
{
    public static W8SaveData w8SaveData;
    public static int playerScore;
    public static string savePath = "/W8SaveData";
    public static W8ScenarioManager scenarioManager;

    public int currentShip;
    public ShipSaveData[] shipSaveDataRefs;
    public ShipSaveData[] shipSaveData;
    public bool[] shipsUnlocked = new bool[4];

    private void Awake()
    {
        Debug.Log("Player Prefs Before: " + PlayerPrefs.GetInt("firstStart"));
        w8SaveData = this;

        //SaveSystem.SaveStats(this);
        if (PlayerPrefs.GetInt("firstStart") == 0)
        {
            shipsUnlocked = new bool[4];
            shipsUnlocked[0] = true;
            SaveSystem.FirstTimeSave();
            PlayerPrefs.SetInt("firstStart", 1);
        }

        Debug.Log("Player Prefs After: " + PlayerPrefs.GetInt("firstStart"));

        shipSaveData = new ShipSaveData[shipSaveDataRefs.Length];
        for (int i = 0; i < shipSaveData.Length; i++)
        {
            shipSaveData[i] = shipSaveDataRefs[i];
        }

        //LoadSave();
    }

    void Start()
    {
        //w8SaveData = this;

        //SaveSystem.SaveStats();
        StartCoroutine("LoadStats");

        LoadSave();
    }

    IEnumerable LoadStats()
    {
        yield return new WaitForSeconds(0.2f);

        //ResetSaveStats();

        LoadSave();
    }

    public void ResetSaveStats()
    {
        playerScore = 0;
        currentShip = 0;

        SaveSystem.SaveStats(this);

        //PlayerData data = SaveSystem.LoadLevel(savePath);

        //data.currentShip = 0;
        //data.shipsUnlocked = new bool[4];
        //data.shipsUnlocked[0] = true;
        //data.shipHull = new int[4];
        //data.primaryWeaponLevel = new int[4];
        //data.secondaryWeaponLevel = new int[4];
        //data.primaryWeaponName = new string[4];
        //data.secondaryWeaponName = new string[4];

        //SaveSystem.SaveStats(this);
    }

    public void LoadSave()
    {
        PlayerData data = SaveSystem.LoadLevel(savePath);

        playerScore = data.w8PlayerCurrency;
        currentShip = data.currentShip;
        //shipSaveData = data.shipSaveData;
        shipsUnlocked = data.shipsUnlocked;

        //for(int i = 0; i < data.shipsUnlocked.Length; i++)
        //{
        //    shipsUnlocked[i] = data.shipsUnlocked[i];
        //}

        //for(int i = 0; i < shipSaveData.Length; i++)
        //{
        //    shipSaveData[i].shipHull = data.shipHull[i];
        //    shipSaveData[i].primaryWeapon.weaponName = data.primaryWeaponName[i];
        //    shipSaveData[i].primaryWeapon.weaponLevel = data.primaryWeaponLevel[i];
        //    shipSaveData[i].secondaryWeapon.weaponName = data.secondaryWeaponName[i];
        //    shipSaveData[i].secondaryWeapon.weaponLevel = data.secondaryWeaponLevel[i];
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            //AddCurrency(100);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            //ResetSaveData();
        }
    }

    void ResetSaveData()
    {
        PlayerPrefs.SetInt("firstStart", 0);

        playerScore = 0;
        currentShip = 0;

        shipsUnlocked = new bool[4];
        shipsUnlocked[0] = true;
        shipsUnlocked[1] = false;
        shipsUnlocked[2] = false;
        shipsUnlocked[3] = false;

        SaveSystem.SaveStats(this);
    }

    public static void AddCurrency(int amount)
    {
        PlayerData data = SaveSystem.LoadLevel(W8SaveData.savePath);

        if (w8SaveData != null)
        {
            w8SaveData.LoadSave();
        }
        playerScore = data.w8PlayerCurrency;

        // Save level stats
        if (data != null)
        {
            playerScore += amount;
            //Debug.Log("Add: " + amount.ToString() + ", Total: " + data.w8PlayerCurrency);

            if (playerScore < 0)
                playerScore = 0;

            //w8SaveData.StartCoroutine("SaveCurrency");
            SaveSystem.SaveStats(w8SaveData);
        }
        else
        {
            Debug.LogError("Could not load " + data);
        }
    }

    public static void AddScenarioCurrency(int amount)
    {
        PlayerData data = SaveSystem.LoadLevel(W8SaveData.savePath);

        if (w8SaveData != null)
        {
            w8SaveData.LoadSave();
        }
        playerScore = data.w8PlayerCurrency;

        // Save level stats
        if (data != null)
        {
            playerScore += amount;
            //Debug.Log("Add: " + amount.ToString() + ", Total: " + data.w8PlayerCurrency);

            if (playerScore < 0)
                playerScore = 0;

            //if(scenarioManager)
            //    scenarioManager.StartCoroutine("SaveCurrency");
            //SaveSystem.SaveStats(w8SaveData);
        }
        else
        {
            Debug.LogError("Could not load " + data);
        }
    }

    public static IEnumerator SaveCurrency()
    {
        yield return new WaitForSeconds(1);

        SaveSystem.SaveStats(w8SaveData);
    }
}
