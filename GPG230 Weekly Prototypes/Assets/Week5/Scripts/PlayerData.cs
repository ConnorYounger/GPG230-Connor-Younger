using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    [System.Serializable]
    public struct levelstats
    {
        public int playerScore;
        public int scoreMultiplier;
        public int playerRank;
    }

    public levelstats[] levels = new levelstats[5];

    // Week 8 data
    public int w8PlayerCurrency;
    private static int numbOfShips = 4;

    // Ship 1 data
    public int currentShip;
    //public ShipSaveData[] shipSaveData;
    public bool[] shipsUnlocked = new bool[numbOfShips];
    public int[] shipHull = new int[numbOfShips];

    public int[] primaryWeaponLevel = new int[numbOfShips];
    public string[] primaryWeaponName = new string[numbOfShips];
    public int[] secondaryWeaponLevel = new int[numbOfShips];
    public string[] secondaryWeaponName = new string[numbOfShips];

    public PlayerData(W5ScoreManager scoreManager, int level)
    {
        if(level < levels.Length)
        {
            levels[level].playerScore = scoreManager.playerScore;
            levels[level].scoreMultiplier = scoreManager.highestMultiplier;
            levels[level].playerRank = scoreManager.playerRank;
        }
    }

    public PlayerData()
    {
        w8PlayerCurrency = W8SaveData.playerScore;
    }

    public PlayerData(W8SaveData saveData)
    {
        w8PlayerCurrency = W8SaveData.playerScore;

        currentShip = saveData.currentShip;
        //shipSaveData = saveData.shipSaveData;
        shipsUnlocked = saveData.shipsUnlocked;

        for (int i = 0; i < saveData.shipSaveData.Length; i++)
        {
            shipHull[i] = saveData.shipSaveData[i].shipHull;
            primaryWeaponName[i] = saveData.shipSaveData[i].primaryWeapon.weaponName;
            primaryWeaponLevel[i] = saveData.shipSaveData[i].primaryWeapon.weaponLevel;
            secondaryWeaponName[i] = saveData.shipSaveData[i].secondaryWeapon.weaponName;
            secondaryWeaponLevel[i] = saveData.shipSaveData[i].secondaryWeapon.weaponLevel;
        }

        Debug.Log(primaryWeaponName[0]);
    }
}
