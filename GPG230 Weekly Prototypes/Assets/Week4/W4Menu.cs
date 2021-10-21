using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class W4Menu : MonoBehaviour
{
    public GameObject cam1;
    public GameObject cam2;

    public GameObject levelUI;

    [Header("Highscore texts")]
    public TMP_Text level1;
    public TMP_Text level2;
    public TMP_Text level3;

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

        UpdateHighscores();
    }

    public void ResetHighScores()
    {
        PlayerPrefs.SetFloat("level1", 9999);
        PlayerPrefs.SetFloat("level2", 9999);
        PlayerPrefs.SetFloat("level3", 9999);

        UpdateHighscores();
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
    }
}
