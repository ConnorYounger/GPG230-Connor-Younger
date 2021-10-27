using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class W5ScoreManager : MonoBehaviour
{
    [Header("Player")]
    public int playerHealth = 5;
    public float hitCoolDown = 1;
    private bool canTakeDamage = true;
    private bool playerLose;

    [Header("Score")]
    public int scoreMultiplier = 5;
    public int negativeScoreMultiplier = 1;
    public int playerScore;
    public string playerRank = "--";

    [Header("Music")]
    public float trackLength = 300;

    [Header("Scene Refrences")]
    public TMP_Text playerScoreText;
    public TMP_Text scoreMultiplierText;

    public GameObject deathUI;
    public GameObject winUI;

    public TMP_Text winFinalScoreText;
    public TMP_Text winFinalMultiplierText;
    public TMP_Text winFinalRankText;

    public TMP_Text loseScoreText;
    public TMP_Text loseMultiplierText;


    void Start()
    {
        UpdateUI();

        StartCoroutine("EndSong");
    }

    public void AddScore(int amount)
    {
        if (!playerLose)
        {
            playerScore += amount * scoreMultiplier;
            scoreMultiplier++;
            negativeScoreMultiplier = 1;

            UpdateUI();
        }
    }

    public void RemoveScore(int amount)
    {
        if (canTakeDamage && !playerLose)
        {
            playerScore -= amount * negativeScoreMultiplier;
            negativeScoreMultiplier++;
            scoreMultiplier = 1;

            if(playerScore < 0)
            {
                playerScore = 0;
            }

            canTakeDamage = false;

            StopCoroutine("ResetHitCoolDown");
            StartCoroutine("ResetHitCoolDown");

            CheckForPlayerLose();
            UpdateUI();
        }
    }

    IEnumerator ResetHitCoolDown()
    {
        yield return new WaitForSeconds(hitCoolDown);

        canTakeDamage = true;
    }

    IEnumerator EndSong()
    {
        yield return new WaitForSeconds(trackLength);

        if(!playerLose)
            PlayerWin();
    }

    void CheckForPlayerLose()
    {
        if(negativeScoreMultiplier >= playerHealth)
        {
            PlayerLose();
        }
    }

    void PlayerLose()
    {
        playerLose = true;

        Time.timeScale = 0;

        if (deathUI)
        {
            deathUI.SetActive(true);

            if (loseScoreText)
                loseScoreText.text = playerScore.ToString();

            if (loseMultiplierText)
                loseMultiplierText.text = scoreMultiplier.ToString();
        }
    }

    void PlayerWin()
    {
        // set save stats

        Time.timeScale = 0;

        if (winUI)
        {
            winUI.SetActive(true);

            if (winFinalScoreText)
            {
                winFinalScoreText.text = playerScore.ToString();
            }

            if (winFinalMultiplierText)
            {
                winFinalMultiplierText.text = scoreMultiplier.ToString();
            }

            if (winFinalRankText)
            {
                // set to level rank
                //winFinalRankText.text = scoreMultiplier.ToString();
            }
        }
    }

    void UpdateUI()
    {
        if (playerScoreText)
        {
            playerScoreText.text = playerScore.ToString();
        }

        if (scoreMultiplierText)
        {
            scoreMultiplierText.text = "x" + scoreMultiplier.ToString();
        }
    }
}
