using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class W9MenuManager : MonoBehaviour
{
    public Button continueButton;

    public GameObject cat;
    public GameObject ai;

    void Start()
    {
        if(PlayerPrefs.GetInt("W9Level") > 1)
        {
            continueButton.interactable = true;
        }
        else
        {
            continueButton.interactable = false;
        }

        if(PlayerPrefs.GetInt("AI1B1") == 1)
        {
            cat.SetActive(true);
        }

        if (PlayerPrefs.GetInt("TrueAI") == 1)
        {
            ai.SetActive(true);
        }
        else
        {
            ai.SetActive(false);
        }
    }

    public void NewGame()
    {
        PlayerPrefs.SetInt("W9Level", 1);
        ContinueGame();
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene("Week9SceneTransition");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
