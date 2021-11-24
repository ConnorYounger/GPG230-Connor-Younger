using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public Image eveImage;
    public Sprite[] eveSprites;

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

    public speaches leaveText;
    public speaches welcomeBackText;

    private bool leaveGame;

    void Start()
    {
        if (PlayerPrefs.GetInt("TrueAI") == 1)
            StartCoroutine("WelcomeBackText");
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


        //int rand = Random.Range(0, dialouge.Length);
        int rand = PlayerPrefs.GetInt("EveDialouge");

        currentDialouge = dialouge[rand];

        rand++;

        if(rand > 27)
        {
            int r = Random.Range(0, catText.Length);
            currentDialouge = catText[r];
        }
        else if(rand < dialouge.Length)
        {
            PlayerPrefs.SetInt("EveDialouge", rand);
        }
        else
        {
            PlayerPrefs.SetInt("EveDialouge", 0);
        }

        textUI.SetActive(true);
        NextText();
    }

    public void NextText()
    {
        newGameButton.interactable = false;

        eveImage.sprite = eveSprites[1];

        textIndex++;
        textUI.SetActive(true);

        if (currentDialouge.text.Length > textIndex)
        {
            textWriter.AddWritter(conversationText, currentDialouge.text[textIndex], textTime, true);
        }
        else
        {
            if (leaveGame)
                Application.Quit();
            else
                EndDialouge();
        }
    }

    public void EndDialouge()
    {
        textIndex = 0;
        textUI.SetActive(false);
        eveImage.sprite = eveSprites[0];
        newGameButton.interactable = true;
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

        if(PlayerPrefs.GetInt("firstTalk") == 1)
        {
            PlayerPrefs.SetInt("firstTalk", 0);
            FirstTalk();
        }
        else
        {
            SayRandomDialouge();
        }
    }

    public void NewGameDialouge()
    {
        if (PlayerPrefs.GetInt("EveDialouge") > 26)
        {
            int r = Random.Range(0, catText.Length);
            currentDialouge = catText[r];
        }
        else
        {
            currentDialouge = newGameDialouge[0];
        }

        textIndex = -1;
        NextText();
    }

    public void NewGameDialouge2()
    {
        if (PlayerPrefs.GetInt("EveDialouge") > 26)
        {
            int r = Random.Range(0, catText.Length);
            currentDialouge = catText[r];
        }
        else
        {
            currentDialouge = newGameDialouge[1];
        }

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

    public void QuitGame()
    {
        leaveGame = true;

        currentDialouge = leaveText;
        textIndex = -1;
        NextText();
    }

    IEnumerator WelcomeBackText()
    {
        yield return new WaitForSeconds(0.5f);

        if (PlayerPrefs.GetInt("EveDialouge") > 26)
        {
            int r = Random.Range(0, catText.Length);
            currentDialouge = catText[r];
        }
        else
        {
            currentDialouge = welcomeBackText;
        }

        textIndex = -1;
        NextText();
    }
}
