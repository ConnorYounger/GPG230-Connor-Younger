using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public bool escPause;

    public GameObject pauseUI;
    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController fPSController;

    public bool lockCurserOnReturn;
    private bool gameIsPaused;

    [Header("Game specific")]
    public PuzzleGun puzzleGun;

    // Start is called before the first frame update
    void Start()
    {
        fPSController = GameObject.Find("Player").GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(escPause)
            ESCInput();
    }

    void ESCInput()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (gameIsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        if (puzzleGun)
            puzzleGun.canUse = false;

        if (fPSController)
            fPSController.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0;
        gameIsPaused = true;
        pauseUI.SetActive(true);
    }

    public void ResumeGame()
    {
        if (lockCurserOnReturn)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        Time.timeScale = 1;
        gameIsPaused = false;
        pauseUI.SetActive(false);

        if (fPSController)
            fPSController.enabled = true;

        StopCoroutine("ResetGameSpecific");
        StartCoroutine("ResetGameSpecific");
    }

    IEnumerator ResetGameSpecific()
    {
        yield return new WaitForSeconds(0.1f);

        if (puzzleGun)
            puzzleGun.canUse = true;
    }
}
