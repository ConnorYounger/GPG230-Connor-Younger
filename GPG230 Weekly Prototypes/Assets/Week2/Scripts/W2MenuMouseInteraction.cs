using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class W2MenuMouseInteraction : MonoBehaviour
{
    public Image exitButton;
    public SceneSwitcher sceneManager;

    public Outline frontDoorOutline;

    public AudioSource openDoorAudioSource;

    public Animator openDoorAnimator;

    public Color exitHighlight;
    public Color exitNormal;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
            }
        }
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
