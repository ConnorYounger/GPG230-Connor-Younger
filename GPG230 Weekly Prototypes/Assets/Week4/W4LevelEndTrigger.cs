using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W4LevelEndTrigger : MonoBehaviour
{
    public GameObject finishLevelUI;
    public GameObject player;

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
        player.SetActive(false);
        finishLevelUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
