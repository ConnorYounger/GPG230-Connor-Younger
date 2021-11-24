using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConversationManager2 : MonoBehaviour
{
    [System.Serializable]
    public struct speaches
    {
        public string[] text;
    }
    public speaches[] dialouge;

    public speaches[] catText;

    public GameObject textUI;
    public TMP_Text conversationText;
    public TextWriter textWriter;
    public float textTime = 0.1f;

    public speaches firstText;
    public speaches[] newGameDialouge;

    public Animator buttonAnimator;
    public Animator buttonAnimator2;

    private int textIndex;
    private speaches currentDialouge;

    public Button newGameButton;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SayRandomDialouge()
    {
        if (buttonAnimator)
        {
            buttonAnimator.Play("CharacterSelectAnimation");
        }

        textIndex = -1;

        int rand = Random.Range(0, dialouge.Length);
        currentDialouge = dialouge[rand];

        textUI.SetActive(true);
        NextText();
    }

    public void NextText()
    {
        textIndex++;
        textUI.SetActive(true);

        if (currentDialouge.text.Length > textIndex)
        {
            textWriter.AddWritter(conversationText, currentDialouge.text[textIndex], textTime, true);
        }
        else
        {
            EndDialouge();
            newGameButton.interactable = true;
        }
    }

    public void EndDialouge()
    {
        textIndex = 0;
        textUI.SetActive(false);
    }

    public void FirstTalk()
    {
        Debug.Log("First Talk");
        currentDialouge = firstText;
        textIndex = -1;
        NextText();
    }

    public void SayDialouge()
    {
        if (buttonAnimator)
        {
            buttonAnimator.Play("CharacterSelectAnimation");
        }

        if(PlayerPrefs.GetInt("firstTalk") == 0)
        {
            PlayerPrefs.SetInt("firstTalk", 1);
            FirstTalk();
        }
        else
        {
            SayRandomDialouge();
        }
    }

    public void NewGameDialouge()
    {
        currentDialouge = newGameDialouge[0];
        textIndex = -1;
        NextText();
    }

    public void NewGameDialouge2()
    {
        currentDialouge = newGameDialouge[1];
        textIndex = -1;
        NextText();
    }

    public void PlayRandomCatText()
    {
        if (buttonAnimator2)
        {
            buttonAnimator2.Play("CharacterSelectAnimation");
        }

        textIndex = -1;

        int rand = Random.Range(0, catText.Length);
        currentDialouge = catText[rand];

        textUI.SetActive(true);
        NextText();
    }
}
