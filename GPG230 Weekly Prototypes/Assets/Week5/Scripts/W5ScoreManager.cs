using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class W5ScoreManager : MonoBehaviour
{
    [Header("Player")]
    public int levelIndex;
    public int playerHealth = 5;
    public float hitCoolDown = 1;
    private bool canTakeDamage = true;
    private bool playerLose;

    [Header("Score")]
    public int scoreMultiplier = 5;
    public int negativeScoreMultiplier = 1;
    public int highestMultiplier = 0;
    public int playerScore;
    public int playerRank;

    [Header("Rank Scores")]
    public int[] rankScores = new int[8] { 0, 1, 1000, 10000, 100000, 1000000, 10000000, 100000000 };

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
    public TMP_Text winHighScoreText;

    public TMP_Text loseScoreText;
    public TMP_Text loseHighScoreText;
    public TMP_Text loseMultiplierText;

    [Header("Animations")]
    public Animator animator;
    public AudioSource audioSource;
    public AudioSource musicAudioSource;
    public AudioClip[] hitSounds;
    public AudioClip deathSound;
    public AudioClip winSound;

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

    public void ResetScores()
    {
        SaveSystem.SaveLevel(this, levelIndex);
    }

    public void RemoveScore(int amount)
    {
        if (canTakeDamage && !playerLose)
        {
            playerScore -= amount * negativeScoreMultiplier;
            negativeScoreMultiplier++;

            if(scoreMultiplier > highestMultiplier)
            {
                highestMultiplier = scoreMultiplier;
            }

            scoreMultiplier = 1;

            if(playerScore < 0)
            {
                playerScore = 0;
            }

            if (animator)
                animator.Play("PlayerHit");

            if (audioSource && hitSounds.Length > 0)
            {
                int rand = Random.Range(0, hitSounds.Length);
                audioSource.clip = hitSounds[rand];
                audioSource.Play();
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
                loseMultiplierText.text = highestMultiplier.ToString();

            if (loseHighScoreText)
            {
                PlayerData data = SaveSystem.LoadLevel(levelIndex);

                loseHighScoreText.text = data.levels[levelIndex].playerScore.ToString();
            }
        }

        if (musicAudioSource)
            musicAudioSource.Stop();

        if (audioSource && deathSound)
        {
            audioSource.clip = deathSound;
            audioSource.Play();
        }
    }

    void PlayerWin()
    {
        highestMultiplier = GetHighestMultiplier();

        PlayerData data = SaveSystem.LoadLevel(levelIndex);

        if (audioSource && winSound)
        {
            audioSource.clip = winSound;
            audioSource.Play();
        }

        if (winFinalScoreText)
        {
            winFinalScoreText.text = playerScore.ToString();
        }

        if (winFinalMultiplierText)
        {
            winFinalMultiplierText.text = highestMultiplier.ToString();
        }

        // Save level stats
        if (data != null)
        {
            // Load level score
            if (data.levels[levelIndex].playerScore > playerScore)
            {
                playerScore = data.levels[levelIndex].playerScore;
            }
            
            // Load level multiplier
            if (data.levels[levelIndex].scoreMultiplier > highestMultiplier)
            {
                highestMultiplier = data.levels[levelIndex].scoreMultiplier;
            }

            RankString();

            SaveSystem.SaveLevel(this, levelIndex);
        }
        else
        {
            Debug.LogError("Could not load " + data);
        }

        Time.timeScale = 0;

        if (winFinalRankText)
        {
            winFinalRankText.text = RankString();
        }

        if (winHighScoreText)
        {
            winHighScoreText.text = playerScore.ToString();
        }

        //Debug.Log("newScore: " + newScore);

        if (winUI)
        {
            winUI.SetActive(true);
        }
    }

    public int GetHighestMultiplier()
    {
        if (scoreMultiplier > highestMultiplier)
        {
            return scoreMultiplier;
        }
        else
        {
            return highestMultiplier;
        }
    }

    public string RankString()
    {
        // Set player rank
        for (int i = 0; i < rankScores.Length; i++)
        {
            if (playerScore > rankScores[i])
            {
                playerRank = i;
            }
        }

        switch (playerRank)
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
