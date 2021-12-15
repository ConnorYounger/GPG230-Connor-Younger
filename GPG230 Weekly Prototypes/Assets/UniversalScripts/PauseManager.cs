using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public bool escPause;

    public GameObject pauseUI;
    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController fPSController;

    public bool lockCurserOnReturn;
    public bool gameIsPaused;
    public bool setTimeScale = true;

    public bool restrictCursor;

    [Header("Game specific")]
    public PuzzleGun puzzleGun;
    public W8ShipMovement shipMovement;
    public ShipWeaponManager weaponManager;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("Player"))
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

        if (shipMovement)
            shipMovement.enabled = false;

        if (weaponManager)
            weaponManager.enabled = false;

        if (fPSController)
            fPSController.enabled = false;

        if (!restrictCursor)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Confined;

        Cursor.visible = true;

        if(setTimeScale)
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

        if(setTimeScale)
            Time.timeScale = 1;

        if (shipMovement)
            shipMovement.enabled = true;

        if (weaponManager)
            weaponManager.enabled = true;

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
