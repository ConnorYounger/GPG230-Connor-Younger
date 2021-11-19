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
    public int currentCharacter;

    public GameObject questionsTab;
    public GameObject conversationTab;
    public GameObject characterChooseTab;

    public TMP_Text characterName;
    public TMP_Text conversationText;
    public TMP_Text questioText;

    private int textIndex;
    private int currentQuestionIndex;

    private bool chooseCharacter;

    private void Start()
    {
        timeSlots = new List<GameObject>();
        currentTime = startingTime;
        UpdateTimeSlots();
    }

    public void CharacterSelect(int i)
    {
        if (!chooseCharacter)
        {
            currentCharacter = i;

            characterName.text = characters[currentCharacter].characterName;
            // Set sprite
        }
        else
        {
            PlayerCharacterSelect(i);
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
                    currentTime -= 2;
                    break;
            }

            currentTime--;

            UpdateTimeSlots();

            conversationText.text = characters[currentCharacter].question[currentQuestionIndex].questionAnswers[textIndex];

            ShowConversationTab();
        }
        else
        {
            NoTimLeft();
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
            conversationText.text = characters[currentCharacter].question[currentQuestionIndex].questionAnswers[textIndex];
        }
        else
        {
            ShowQuestionsTab();
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
            Debug.Log("Player Win");
        }
        else
        {
            Debug.Log("Player Lose");
        }
    }
}