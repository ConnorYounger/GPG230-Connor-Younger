using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class W5PauseLevel : MonoBehaviour
{
    public bool canPause = true;
    private bool isPaused;

    public W5ScoreManager scoreManager;

    [Header("UIRefrecnes")]
    public GameObject pauseUI;
    public TMP_Text playerScore;
    public TMP_Text highScore;
    public TMP_Text highestmultiplier;
    public TMP_Text playerRank;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if (!isPaused && canPause)
        {
            isPaused = true;
            pauseUI.SetActive(true);

            scoreManager.musicAudioSource.Pause();

            Time.timeScale = 0;

            UpdateUI();
        }
    }

    void UpdateUI()
    {
        PlayerData data = SaveSystem.LoadLevel(scoreManager.levelIndex);

        playerScore.text = scoreManager.playerScore.ToString();
        highScore.text = data.levels[scoreManager.levelIndex].playerScore.ToString();
        highestmultiplier.text = scoreManager.GetHighestMultiplier().ToString();
        playerRank.text = scoreManager.RankString();
    }

    public void ResmumeGame()
    {
        if (isPaused && canPause)
        {
            isPaused = false;
            pauseUI.SetActive(false);

            scoreManager.musicAudioSource.UnPause();

            Time.timeScale = 1;
        }
    }
}
