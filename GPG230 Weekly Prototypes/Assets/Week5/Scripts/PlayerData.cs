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
        public string playerRank;
    }

    public levelstats[] levels;

    public PlayerData(W5ScoreManager scoreManager, int level)
    {
        if(level < levels.Length)
        {
            levels[level].playerScore = scoreManager.playerScore;
            levels[level].scoreMultiplier = scoreManager.scoreMultiplier;
            levels[level].playerRank = scoreManager.playerRank;
        }
    }
}
