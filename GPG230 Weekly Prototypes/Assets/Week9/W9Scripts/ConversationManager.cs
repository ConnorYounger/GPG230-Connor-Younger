using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ConversationManager : MonoBehaviour
{
    public int robotCharacterIndex;
    public int levelIndex;

    public int startingTime = 10;
    public Transform timeSlotGroup;
    public GameObject timeSlotPrefab;
    private int currentTime;
    private List<GameObject> timeSlots;
    public Animator timeSlotAnimator;
    public GameObject outOfTimeText;

    public CharacterStats[] characters;
    public CharacterStats aIChar1;
    public CharacterStats aIChar2;
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
    public GameObject extraOptionButton;
    public GameObject theChoiceTab;

    public TMP_Text characterName;
    public TMP_Text characterName2;
    public TMP_Text conversationText;
    public TMP_Text questioText;
    public TMP_Text confirmSelectionText;
    public TMP_Text bestFriendText;

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

        if(bestFriendText && PlayerPrefs.GetInt("AI2B3") == 1)
        {
            bestFriendText.text = "Best Friend";
        }
    }

    public void CharacterSelect(int i)
    {
        currentCharacter = i;

        if (!chooseCharacter)
        {
            if(characterName)
                characterName.text = characters[currentCharacter].characterName;
            if(characterName2)
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
            //Debug.Log("All buttons are false");
            questionButtons[3].interactable = true;
        }
        else
        {
            //Debug.Log("At least one button is true");
            questionButtons[3].interactable = false;
        }

        if (extraOptionButton)
        {
            if (levelIndex == 3 && currentCharacter == 3)
            {
                extraOptionButton.SetActive(true);
            }
            else
            {
                extraOptionButton.SetActive(false);
            }
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

            if(i < characters[currentCharacter].answeredQuestion.Length)
                characters[currentCharacter].answeredQuestion[i] = false;

            UpdateButtonInterativity();

            //conversationText.text = characters[currentCharacter].question[currentQuestionIndex].questionAnswers[textIndex];
            if (currentCharacter != 3)
            {
                StartTextWritting(i);
            }
            else
            {
                switch (levelIndex)
                {
                    case 1:
                        StartTextWritting(i);
                        PlayerPrefs.SetInt("AI1B" + i.ToString(), 1);
                        break;
                    case 2:
                        if(PlayerPrefs.GetInt("AI1B" + i.ToString()) == 0)
                        {
                            textWriter.AddWritter(conversationText, aIChar1.question[currentQuestionIndex].questionAnswers[textIndex], textTime, true);

                            if (aIChar1.question[currentQuestionIndex].alternateSprites.Length > textIndex && aIChar1.question[currentQuestionIndex].alternateSprites[textIndex] != null)
                            {
                                characterButtons[currentCharacter].GetComponent<Image>().sprite = aIChar1.question[currentQuestionIndex].alternateSprites[textIndex];
                            }
                        }
                        else
                        {
                            StartTextWritting(i);
                            PlayerPrefs.SetInt("AI2B" + i.ToString(), 1);
                        }
                        break;
                    case 3:
                        if (PlayerPrefs.GetInt("AI1B" + i.ToString()) == 0)
                        {
                            textWriter.AddWritter(conversationText, aIChar1.question[currentQuestionIndex].questionAnswers[textIndex], textTime, true);

                            if (aIChar1.question[currentQuestionIndex].alternateSprites.Length > textIndex && aIChar1.question[currentQuestionIndex].alternateSprites[textIndex] != null)
                            {
                                characterButtons[currentCharacter].GetComponent<Image>().sprite = aIChar1.question[currentQuestionIndex].alternateSprites[textIndex];
                            }
                        }
                        else if(PlayerPrefs.GetInt("AI2B" + i.ToString()) == 0)
                        {
                            textWriter.AddWritter(conversationText, aIChar2.question[currentQuestionIndex].questionAnswers[textIndex], textTime, true);

                            if (aIChar2.question[currentQuestionIndex].alternateSprites.Length > textIndex && aIChar2.question[currentQuestionIndex].alternateSprites[textIndex] != null)
                            {
                                characterButtons[currentCharacter].GetComponent<Image>().sprite = aIChar2.question[currentQuestionIndex].alternateSprites[textIndex];
                            }
                        }
                        else
                        {
                            StartTextWritting(i);
                        }
                        break;
                }
            }

            Debug.Log("AI" + levelIndex.ToString() + "B" + i.ToString() + ": " + PlayerPrefs.GetInt("AI" + levelIndex.ToString() + "B" + i.ToString()));

            ShowConversationTab();
            DisableOtherCharacterButtons();
        }
        else
        {
            NoTimLeft();
        }
    }

    public void FakeButton()
    {
        currentTime += 3;
        UpdateTimeSlots();
    }

    void StartTextWritting(int i)
    {
        textWriter.AddWritter(conversationText, characters[currentCharacter].question[currentQuestionIndex].questionAnswers[textIndex], textTime, true);

        if (characters[currentCharacter].question[currentQuestionIndex].alternateSprites.Length > textIndex && characters[currentCharacter].question[currentQuestionIndex].alternateSprites[textIndex] != null)
        {
            characterButtons[currentCharacter].GetComponent<Image>().sprite = characters[currentCharacter].question[currentQuestionIndex].alternateSprites[textIndex];
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

        timeSlotAnimator.Play("OutOfTime");
        outOfTimeText.SetActive(true);

        // Play sound

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
            if(levelIndex == 3 && currentCharacter == 3 && currentQuestionIndex == 3 && PlayerPrefs.GetInt("AI2B3") == 1)
            {
                theChoiceTab.SetActive(true);
            }
            else
            {
                ShowQuestionsTab();
                characterButtons[currentCharacter].GetComponent<Image>().sprite = characters[currentCharacter].characterSprite;
            }
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
        if (timeSlotPrefab && timeSlotGroup)
        {
            foreach (GameObject slot in timeSlots)
            {
                Destroy(slot, 0.1f);
            }

            timeSlots.Clear();

            for (int i = 0; i < currentTime; i++)
            {
                GameObject newSlot = Instantiate(timeSlotPrefab, timeSlotGroup.position, Quaternion.identity);
                newSlot.transform.parent = timeSlotGroup;
                timeSlots.Add(newSlot);
            }
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

    public void SheIsTheAI()
    {
        SceneManager.LoadScene("Week9MainMenu");
    }

    public void SheMeantSomething()
    {
        PlayerPrefs.SetInt("TrueAI", 1);
        SceneManager.LoadScene("Week9MainMenu");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Week9MainMenu");
    }

    public void NextLevel()
    {
        PlayerPrefs.SetInt("W9Level", levelIndex + 1);
        SceneManager.LoadScene("Week9SceneTransition");
    }
}