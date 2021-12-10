using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class W9TransitionManager : MonoBehaviour
{
    public GameObject introUI;
    public TMP_Text introText;
    public string introTextString;
    public float introTextTime;
    public TMP_Text titleText;
    public string introText2String;
    public string outroTestString;
    public TMP_Text nextWeekText;
    public float weekTextTime;
    public GameObject nextWeekButton;
    public TextWriter textWriter;

    public GameObject eveMessage;

    private int index;

    void Start()
    {
        if (PlayerPrefs.GetInt("W9Level") == 1)
        {
            introUI.SetActive(true);
            textWriter.AddWritter(introText, introTextString, introTextTime, true);
        }
        else if (PlayerPrefs.GetInt("W9Level") == 4)
        {
            introUI.SetActive(true);
            titleText.text = "Congratulations!";
            textWriter.AddWritter(introText, outroTestString, introTextTime, true);
            eveMessage.SetActive(true);
        }
        else
        {
            introUI.SetActive(false);
            nextWeekButton.SetActive(true);
            textWriter.AddWritter(nextWeekText, "Week " + PlayerPrefs.GetInt("W9Level").ToString(), weekTextTime, true);
        }

        PlayerPrefs.SetInt("firstLaunch", 1);
    }

    public void ContinueButton()
    {
        if (PlayerPrefs.GetInt("W9Level") == 2)
        {
            SceneManager.LoadScene("Week9Scene2");
        }
        else if (PlayerPrefs.GetInt("W9Level") == 3)
        {
            SceneManager.LoadScene("Week9Scene3");
        }
        else if (PlayerPrefs.GetInt("W9Level") == 4)
        {
            SceneManager.LoadScene("Week9MainMenu");
        }
        else
        {
            if(index == 0)
            {
                index++;
                titleText.text = "Your Story";
                textWriter.AddWritter(introText, introText2String, introTextTime, true);
            }
            else
                SceneManager.LoadScene("Week9Scene1");
        }
    }
}
