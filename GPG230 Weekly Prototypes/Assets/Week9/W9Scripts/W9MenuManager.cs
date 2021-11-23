using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class W9MenuManager : MonoBehaviour
{
    public Button continueButton;

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
