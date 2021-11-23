using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConversationManager : MonoBehaviour
{
    public int robotCharacterIndex;

    public int startingTime = 10;
    public Transform timeSlotGroup;
    public GameObject timeSlotPrefab;
    private int currentTime;
    private List<GameObject> timeSlots;

    public CharacterStats[] characters;
    public Button[] characterButtons;
    public Button[] questionButtons;
    public Animator[] buttonAnimators;
    public int currentCharacter;

    public GameObject questionsTab;
    public GameObject conversationTab;
    public GameObject characterChooseTab;
    public GameObject confirmSelectionTab;
    public GameObject winTab;
    public GameObject loseTab;

    public TMP_Text characterName;
    public TMP_Text characterName2;
    public TMP_Text conversationText;
    public TMP_Text questioText;
    public TMP_Text confirmSelectionText;

    private int textIndex;
    private int currentQuestionIndex;

    private bool chooseCharacter;

    public TextWriter textWriter;
    public float textTime = 0.1f;

    private void Start()
    {
        timeSlots = new List<GameObject>();
        currentTime = startingTime;
        UpdateTimeSlots();
        CharacterSelect(0);
        SetCharacterSprites();
        UpdateButtonInterativity();
    }

    void SetCharacterSprites()
    {
        for(int i = 0; i < characterButtons.Length; i++)
        {
            if (characterButtons[i].GetComponent<Image>() && characters[i].characterSprite != null)
            {
                characterButtons[i].GetComponent<Image>().sprite = characters[i].characterSprite;
            }
        }

        for (int i = 0; i < characters.Length; i++)
        {
            for (int i2 = 0; i2 < characters[i].answeredQuestion.Length; i2++)
            {
                characters[i].answeredQuestion[i2] = true;
            }
        }
    }

    public void CharacterSelect(int i)
    {
        currentCharacter = i;

        if (!chooseCharacter)
        {
            characterName.text = characters[currentCharacter].characterName;
            characterName2.text = characters[currentCharacter].characterName;

            buttonAnimators[currentCharacter].Play("CharacterSelectAnimation");
            UpdateButtonInterativity();

            // Set sprite
        }
        else
        {
            confirmSelectionText.text = "You think " + characters[currentCharacter].characterName + " is the AI.";
            questionsTab.SetActive(false);
            characterChooseTab.SetActive(false);
            confirmSelectionTab.SetActive(true);
            //PlayerCharacterSelect(i);
        }
    }

    void UpdateButtonInterativity()
    {
        for(int i = 0; i < questionButtons.Length; i++)
        {
            questionButtons[i].interactable = characters[currentCharacter].answeredQuestion[i];
        }

        if(!characters[currentCharacter].answeredQuestion[0] && !characters[currentCharacter].answeredQuestion[1] && !characters[currentCharacter].answeredQuestion[2] && characters[currentCharacter].answeredQuestion[3])
        {
            Debug.Log("All buttons are false");
            questionButtons[3].interactable = true;
        }
        else
        {
            Debug.Log("At least one button is true");
            questionButtons[3].interactable = false;
        }
    }

    public void AskQuestion(int i)
    {
        if (currentTime > 0)
        {
            currentQuestionIndex = i;
            textIndex = 0;

            switch (i)
            {
                case 0:
                    questioText.text = "Ask about their childhood";
                    break;
                case 1:
                    questioText.text = "Ask about their relationships";
                    break;
                case 2:
                    questioText.text = "Ask about their dreams / goals";
                    break;
                case 3:
                    questioText.text = "Conversation";
                    currentTime++;
                    break;
            }

            currentTime--;

            UpdateTimeSlots();

            characters[currentCharacter].answeredQuestion[i] = false;
            UpdateButtonInterativity();

            //conversationText.text = characters[currentCharacter].question[currentQuestionIndex].questionAnswers[textIndex];
            textWriter.AddWritter(conversationText, characters[currentCharacter].question[currentQuestionIndex].questionAnswers[textIndex], textTime, true);
            
            if(characters[currentCharacter].question[currentQuestionIndex].alternateSprites.Length > textIndex && characters[currentCharacter].question[currentQuestionIndex].alternateSprites[textIndex] != null)
            {
                characterButtons[currentCharacter].GetComponent<Image>().sprite = characters[currentCharacter].question[currentQuestionIndex].alternateSprites[textIndex];
            }

            ShowConversationTab();
            DisableOtherCharacterButtons();
        }
        else
        {
            NoTimLeft();
        }
    }

    void DisableCharacterButtons()
    {
        foreach(Button b in characterButtons)
        {
            b.interactable = false;
        }
    }

    void DisableOtherCharacterButtons()
    {
        for(int i = 0; i < characterButtons.Length; i++)
        {
            if(i != currentCharacter)
            {
                characterButtons[i].interactable = false;
            }
        }
    }

    void EnableCharacterButtons()
    {
        foreach (Button b in characterButtons)
        {
            b.interactable = true;
        }
    }

    void NoTimLeft()
    {
        Debug.Log("Not time left, must guess");
    }

    public void NextText()
    {
        textIndex++;

        if(characters[currentCharacter].question[currentQuestionIndex].questionAnswers.Length > textIndex)
        {
            //conversationText.text = characters[currentCharacter].question[currentQuestionIndex].questionAnswers[textIndex];
            textWriter.AddWritter(conversationText, characters[currentCharacter].question[currentQuestionIndex].questionAnswers[textIndex], textTime, true);

            if (characters[currentCharacter].question[currentQuestionIndex].alternateSprites.Length > textIndex && characters[currentCharacter].question[currentQuestionIndex].alternateSprites[textIndex] != null)
            {
                characterButtons[currentCharacter].GetComponent<Image>().sprite = characters[currentCharacter].question[currentQuestionIndex].alternateSprites[textIndex];
            }
        }
        else
        {
            ShowQuestionsTab();
            characterButtons[currentCharacter].GetComponent<Image>().sprite = characters[currentCharacter].characterSprite;
        }
    }

    public void ShowConversationTab()
    {
        questionsTab.SetActive(false);
        conversationTab.SetActive(true);
    }

    public void ShowQuestionsTab()
    {
        conversationTab.SetActive(false);
        questionsTab.SetActive(true);
        EnableCharacterButtons();
    }

    public void UpdateTimeSlots()
    {
        foreach(GameObject slot in timeSlots)
        {
            Destroy(slot, 0.1f);
        }

        timeSlots.Clear();

        for(int i = 0; i < currentTime; i++)
        {
            GameObject newSlot = Instantiate(timeSlotPrefab, timeSlotGroup.position, Quaternion.identity);
            newSlot.transform.parent = timeSlotGroup;
            timeSlots.Add(newSlot);
        }
    }

    public void StartChooseCharacter()
    {
        chooseCharacter = true;

        questionsTab.SetActive(false);
        characterChooseTab.SetActive(true);
    }

    public void StopChooseCharacter()
    {
        chooseCharacter = false;

        characterChooseTab.SetActive(false);
        questionsTab.SetActive(true);
    }

    void PlayerCharacterSelect(int i)
    {
        if(i == robotCharacterIndex)
        {
            confirmSelectionTab.SetActive(false);
            winTab.SetActive(true);
        }
        else
        {
            confirmSelectionTab.SetActive(false);
            loseTab.SetActive(true);
        }
    }

    public void ConfirmSelection()
    {
        PlayerCharacterSelect(currentCharacter);
    }

    public void CancelSelection()
    {
        chooseCharacter = false;

        confirmSelectionTab.SetActive(false);
        questionsTab.SetActive(true);
    }
}