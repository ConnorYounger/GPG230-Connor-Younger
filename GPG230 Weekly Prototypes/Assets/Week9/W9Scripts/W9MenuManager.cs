using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class W9MenuManager : MonoBehaviour
{
    public Button newButton;
    public Button continueButton;

    public GameObject cat;
    public GameObject ai;

    public ConversationManager2 convoManager;

    private int pressedNewGame;

    void Start()
    {
        if(PlayerPrefs.GetInt("W9Level") > 1)
        {
            if (PlayerPrefs.GetInt("TrueAI") == 0)
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetSave();
        }
    }

    public void NewGame()
    {
        if (PlayerPrefs.GetInt("TrueAI") == 0)
        {
            PlayerPrefs.SetInt("W9Level", 1);
            ContinueGame();
        }
        else if (pressedNewGame == 0)
        {
            newButton.interactable = false;
            pressedNewGame = 1;
            convoManager.NewGameDialouge();
        }
        else if(pressedNewGame == 1)
        {
            newButton.interactable = false;
            pressedNewGame = 2;
            convoManager.NewGameDialouge2();
        }
        else
        {
            ResetSave();
            ContinueGame();
        }
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene("Week9SceneTransition");
    }

    public void QuitGame()
    {
        if (PlayerPrefs.GetInt("TrueAI") == 0)
            Application.Quit();
        else
        {
            convoManager.QuitGame();
        }
    }

    void ResetSave()
    {
        PlayerPrefs.SetInt("W9Level", 1);
        PlayerPrefs.SetInt("TrueAI", 0);
        PlayerPrefs.SetInt("AI1B1", 0);

        PlayerPrefs.SetInt("AI1B0", 0);
        PlayerPrefs.SetInt("AI1B1", 0);
        PlayerPrefs.SetInt("AI1B2", 0);
        PlayerPrefs.SetInt("AI1B3", 0);

        PlayerPrefs.SetInt("AI2B0", 0);
        PlayerPrefs.SetInt("AI2B1", 0);
        PlayerPrefs.SetInt("AI2B2", 0);
        PlayerPrefs.SetInt("AI2B3", 0);

        PlayerPrefs.SetInt("AI3B0", 0);
        PlayerPrefs.SetInt("AI3B1", 0);
        PlayerPrefs.SetInt("AI3B2", 0);
        PlayerPrefs.SetInt("AI3B3", 0);

        PlayerPrefs.SetInt("firstTalk", 0);
        PlayerPrefs.SetInt("EveDialouge", 0);
        pressedNewGame = 0;
    }
}
