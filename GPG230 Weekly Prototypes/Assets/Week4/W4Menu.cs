using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class W4Menu : MonoBehaviour
{
    public GameObject cam1;
    public GameObject cam2;

    public GameObject levelUI;

    [Header("Level Buttons")]
    public Button story2Button;
    public Button story3Button;
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;

    [Header("Highscore texts")]
    public TMP_Text level1;
    public TMP_Text level2;
    public TMP_Text level3;
    public TMP_Text end1;
    public TMP_Text end2;

    public void ReturnToMain()
    {
        cam1.SetActive(true);
        cam2.SetActive(false);
        levelUI.SetActive(false);
    }

    public void MoveToLevelUI()
    {
        cam2.SetActive(true);
        cam1.SetActive(false);

        StopCoroutine("ShowLevelUI");
        StartCoroutine("ShowLevelUI");
    }

    IEnumerator ShowLevelUI()
    {
        yield return new WaitForSeconds(1);

        levelUI.SetActive(true);

        CheckLevelUnlocks();
        UpdateHighscores();
    }

    public void ResetHighScores()
    {
        PlayerPrefs.SetFloat("level1", 9999);
        PlayerPrefs.SetFloat("level2", 9999);
        PlayerPrefs.SetFloat("level3", 9999);
        PlayerPrefs.SetFloat("endStory1", 9999);
        PlayerPrefs.SetFloat("endStory2", 9999);

        UpdateHighscores();
    }

    void CheckLevelUnlocks()
    {
        if (PlayerPrefs.GetInt("level1Unlocked") == 0)
        {
            level1Button.interactable = false;
        }
        else
        {
            level1Button.interactable = true;
        }

        if (PlayerPrefs.GetInt("level2Unlocked") == 0)
        {
            level2Button.interactable = false;
        }
        else
        {
            level2Button.interactable = true;
        }

        if (PlayerPrefs.GetInt("level3Unlocked") == 0)
        {
            level3Button.interactable = false;
        }
        else
        {
            level3Button.interactable = true;
        }

        if (PlayerPrefs.GetInt("story2Unlocked") == 0)
        {
            story2Button.interactable = false;
        }
        else
        {
            story2Button.interactable = true;
        }

        if (PlayerPrefs.GetInt("story3Unlocked") == 0)
        {
            story3Button.interactable = false;
        }
        else
        {
            story3Button.interactable = true;
        }
    }

    void UpdateHighscores()
    {
        if (level1)
        {
            if(PlayerPrefs.GetFloat("level1") == 9999)
                level1.text = "Best Time: --";
            else
                level1.text = "Best Time: " + Mathf.RoundToInt(PlayerPrefs.GetFloat("level1")).ToString() + "s";
        }

        if (level2)
        {
            if (PlayerPrefs.GetFloat("level2") == 9999)
                level2.text = "Best Time: --";
            else
                level2.text = "Best Time: " + Mathf.RoundToInt(PlayerPrefs.GetFloat("level2")).ToString() + "s";
        }

        if (level3)
        {
            if (PlayerPrefs.GetFloat("level3") == 9999)
                level3.text = "Best Time: --";
            else
                level3.text = "Best Time: " + Mathf.RoundToInt(PlayerPrefs.GetFloat("level3")).ToString() + "s";
        }

        if (end1)
        {
            if (PlayerPrefs.GetFloat("endStory1") == 9999)
                end1.text = "Ending 1: Unachieved";
            else
                end1.text = "Ending 1: Rocket Win";
        }

        if (end2)
        {
            if (PlayerPrefs.GetFloat("endStory2") == 9999)
                end2.text = "Ending 2: Unachieved";
            else
                end2.text = "Ending 2: YANA Win";
        }
    }
}
