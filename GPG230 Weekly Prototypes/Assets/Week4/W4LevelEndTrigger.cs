using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W4LevelEndTrigger : MonoBehaviour
{
    public GameObject finishLevelUI;
    public GameObject player;
    public PuzzleGun puzzleGun;

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
    }
}
