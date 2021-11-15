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

    // Ship 1 data
    public int currentShip;
    public ShipSaveData[] shipSaveData;

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
}
