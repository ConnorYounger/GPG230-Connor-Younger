using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class W9TutorialManager : MonoBehaviour
{
    public int tutorialIndex;

    public GameObject[] tutorialUI;

    public Button[] characterButtons;
    public Button[] questionButtons;
    public Button guessButton;

    public GameObject guessConfirmUI;
    public GameObject finishTutorialUI;

    [Header("Tutorial Conversations")]
    public GameObject buttonPannel;
    public GameObject textPannel;
    public TMP_Text conversationText;
    public TextWriter textWriter;

    public float writeTime = 0.02f;
    public string[] textStrings;

    void Start()
    {
        EnableCharacterButtons();
    }

    void Update()
    {
        
    }

    public void TutorialButtonPress()
    {
        tutorialIndex++;

        switch (tutorialIndex)
        {
            case 1:
                buttonPannel.SetActive(true);
                DisableCharacterButtons();
                break;
            case 2:
                buttonPannel.SetActive(false);
                textPannel.SetActive(true);
                textWriter.AddWritter(conversationText, textStrings[0], writeTime, true);
                break;
            case 3:
                DisableQuestionButtons();
                buttonPannel.SetActive(true);
                textPannel.SetActive(false);
                break;
            case 4:
                buttonPannel.SetActive(false);
                textPannel.SetActive(true);
                textWriter.AddWritter(conversationText, textStrings[1], writeTime, true);
                break;
            case 5:
                buttonPannel.SetActive(true);
                textPannel.SetActive(false);
                questionButtons[3].interactable = false;
                guessButton.interactable = true;
                break;
            case 6:
                buttonPannel.SetActive(false);
                EnableCharacterButtons();
                break;
            case 7:
                buttonPannel.SetActive(false);
                guessConfirmUI.SetActive(true);
                break;
            case 8:
                finishTutorialUI.SetActive(true);
                guessConfirmUI.SetActive(false);
                break;
        }

        for(int i = 0; i < tutorialUI.Length; i++)
        {
            if (i == tutorialIndex)
                tutorialUI[i].SetActive(true);
            else
                tutorialUI[i].SetActive(false);
        }
    }

    void EnableCharacterButtons()
    {
        foreach(Button b in characterButtons)
        {
            b.interactable = true;
        }
    }

    void DisableCharacterButtons()
    {
        foreach (Button b in characterButtons)
        {
            b.interactable = false;
        }
    }

    void DisableQuestionButtons()
    {
        for(int i = 0; i < questionButtons.Length; i++)
        {
            if (i == 3)
            {
                questionButtons[i].interactable = true;
            }
            else
                questionButtons[i].interactable = false;
        }
    }

    public void EndTutorial()
    {
        SceneManager.LoadScene("Week9MainMenu");
    }

    public void RestartTutorial()
    {
        SceneManager.LoadScene("Week9TutorialScene");
    }
}
