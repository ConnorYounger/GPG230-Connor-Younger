using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class W5LevelStatsLoader : MonoBehaviour
{
    [System.Serializable]
    public struct levelRefrences
    {
        public TMP_Text rankText;
        public TMP_Text highScoreText;
        public TMP_Text highMultiplierText;
    }

    public levelRefrences[] levels;

    private void Start()
    {
        LoadLevelStats();
    }

    public void LoadLevelStats()
    {
        //if (SaveSystem.GetSaveFiles()) { }

        for(int i = 0; i < levels.Length; i++)
        {
            PlayerData data = SaveSystem.LoadLevel(i);

            if (data.levels.Length > i)
            {
                levels[i].highScoreText.text = data.levels[i].playerScore.ToString();
                levels[i].highMultiplierText.text = data.levels[i].scoreMultiplier.ToString();
                levels[i].rankText.text = RankString(data.levels[i].playerRank);
            }
        }
    }

    string RankString(int rank)
    {
        Debug.Log(rank);

        switch (rank)
        {
            case 0:
                return "--";
                break;
            case 1:
                return "E";
                break;
            case 2:
                return "D";
                break;
            case 3:
                return "C";
                break;
            case 4:
                return "B";
                break;
            case 5:
                return "A";
                break;
            case 6:
                return "S";
                break;
            case 7:
                return "SS";
                break;
            default:
                return null;
        }
    }

    public void ResetLevelStats()
    {
        for (int i = 0; i < levels.Length; i++){
            SaveSystem.SaveLevel(new W5ScoreManager(), i);
        }

        for (int i = 0; i < levels.Length; i++)
        {
            PlayerData data = SaveSystem.LoadLevel(i);

            if (data.levels.Length < i)
            {
                levels[i].highScoreText.text = data.levels[i].playerScore.ToString();
                levels[i].highMultiplierText.text = data.levels[i].scoreMultiplier.ToString();
                levels[i].rankText.text = RankString(data.levels[i].playerRank);
            }
        }

        LoadLevelStats();
    }
}
