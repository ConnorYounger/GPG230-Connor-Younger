using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class W2MenuMouseInteraction : MonoBehaviour
{
    public Image exitButton;
    public SceneSwitcher sceneManager;

    public Outline frontDoorOutline;
    public Image helpImage;
    public Image restartImage;
    public TMP_Text resetText;

    public GameObject helpUI;
    public W2AchievementsManager achivManager;

    public AudioSource openDoorAudioSource;

    public Animator openDoorAnimator;

    public Color exitHighlight;
    public Color exitNormal;

    private bool canClick;

    // Start is called before the first frame update
    void Start()
    {
        canClick = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(canClick)
            MouseClick();
    }

    void MouseClick()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider != null && hit.collider.name == "ExitButton")
            {
                exitButton.color = exitHighlight;

                StopCoroutine("DefultExitColour");
                StartCoroutine("DefultExitColour");
            }

            if (hit.collider != null && hit.collider.name == "HelpButton")
            {
                helpImage.color = exitHighlight;
            }
            else
            {
                helpImage.color = exitNormal;
            }

            if (hit.collider != null && hit.collider.name == "RestartButton")
            {
                restartImage.color = exitHighlight;
                resetText.text = "Reset Achievements";
            }
            else
            {
                restartImage.color = exitNormal;
                resetText.text = "";
            }

            if (hit.collider != null && hit.collider.name == "FrontDoor")
            {
                frontDoorOutline.enabled = true;
                openDoorAnimator.SetInteger("doorStage", 1);
            }
            else
            {
                frontDoorOutline.enabled = false;
                openDoorAnimator.SetInteger("doorStage", 0);
            }

            if (Input.GetButtonDown("Fire1") && hit.collider != null )
            {
                if (hit.collider.name == "ExitButton")
                {
                    sceneManager.QuitGame();
                }
                else if(hit.collider.name == "FrontDoor")
                {
                    StartCoroutine("OpenDoor");
                }
                else if (hit.collider.name == "HelpButton")
                {
                    OpenHelpMenu();
                }
                else if (hit.collider.name == "RestartButton")
                {
                    RestartAchievements();
                }
            }
        }
    }

    void RestartAchievements()
    {
        PlayerPrefs.SetInt("keyWin", 0);
        PlayerPrefs.SetInt("axeWin", 0);
        PlayerPrefs.SetInt("ladderWin", 0);
        PlayerPrefs.SetInt("allWin", 0);
        PlayerPrefs.SetInt("secret1", 0);
        PlayerPrefs.SetInt("secret2", 0);
    }

    void OpenHelpMenu()
    {
        canClick = false;

        achivManager.UpdateAchivementIcons();
        helpUI.SetActive(true);
    }

    public void CloseHelpMenu()
    {
        helpUI.SetActive(false);

        canClick = true;
    }

    IEnumerator DefultExitColour()
    {
        yield return new WaitForSeconds(0.1f);

        exitButton.color = exitNormal;
    }

    IEnumerator OpenDoor()
    {
        openDoorAnimator.SetInteger("doorStage", 2);
        openDoorAudioSource.Play();

        yield return new WaitForSeconds(1);

        sceneManager.SwitchScene("Week2Scene");
    }
}
