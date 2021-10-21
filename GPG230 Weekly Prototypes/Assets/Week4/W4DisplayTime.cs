using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class W4DisplayTime : MonoBehaviour
{
    [System.Serializable] public enum levels { defult, level1, level2, level3, endStory };
    public levels level;

    private GameObject player;
    private W4LevelTimer timer;

    public TMP_Text highScoreText;
    public TMP_Text levelScoreText;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        timer = player.GetComponent<W4LevelTimer>();

        UpdateStats();
    }

    private void OnEnable()
    {
        //UpdateStats();
    }

    void UpdateStats()
    {
        if (timer) 
        {
            if (level != levels.defult)
            {
                switch (level)
                {
                    case levels.level1:
                        if (timer.levelTimer < PlayerPrefs.GetFloat("level1"))
                        {
                            PlayerPrefs.SetFloat("level1", timer.levelTimer);
                        }
                        break;
                    case levels.level2:
                        if (timer.levelTimer < PlayerPrefs.GetFloat("level2"))
                        {
                            PlayerPrefs.SetFloat("level2", timer.levelTimer);
                        }
                        break;
                    case levels.level3:
                        if (timer.levelTimer < PlayerPrefs.GetFloat("level3"))
                        {
                            PlayerPrefs.SetFloat("level3", timer.levelTimer);
                        }
                        break;
                }

                ShowStats(level.ToString());
            }
        }
    }

    public void ShowStats(string level)
    {
        if (highScoreText)
        {
            highScoreText.text = "High Score: " + Mathf.RoundToInt(PlayerPrefs.GetFloat(level)).ToString() + "s";
        }

        if (levelScoreText)
        {
            levelScoreText.text = "Level Score: " + Mathf.RoundToInt(timer.levelTimer).ToString() + "s";
        }
    }

}
