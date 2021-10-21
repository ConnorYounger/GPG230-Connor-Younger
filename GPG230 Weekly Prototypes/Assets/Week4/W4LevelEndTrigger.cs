using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W4LevelEndTrigger : MonoBehaviour
{
    public GameObject finishLevelUI;
    public GameObject player;
    public PuzzleGun puzzleGun;

    [System.Serializable] public enum levels { defult, level1, level2, level3, story1, story2 };
    public levels level;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            ReachGoal();
        }
    }

    void ReachGoal()
    {
        if(player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>())
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;

        if (puzzleGun)
            puzzleGun.enabled = false;

        finishLevelUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        switch (level)
        {
            case levels.level1:
                PlayerPrefs.SetInt("level2Unlocked", 1);
                break;
            case levels.level2:
                PlayerPrefs.SetInt("story2Unlocked", 1);
                break;
            case levels.level3:
                PlayerPrefs.SetInt("story3Unlocked", 1);
                break;
            case levels.story1:
                PlayerPrefs.SetInt("level1Unlocked", 1);
                break;
            case levels.story2:
                PlayerPrefs.SetInt("level3Unlocked", 1);
                break;
        }
    }
}
