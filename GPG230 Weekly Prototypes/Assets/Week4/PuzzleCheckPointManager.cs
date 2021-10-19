using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCheckPointManager : MonoBehaviour
{
    public Transform resetPoint;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        if (!player)
        {
            player = GameObject.Find("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && resetPoint)
        {
            CheckPoint();
        }
    }

    void CheckPoint()
    {
        Debug.Log("Restart Checkpoint");

        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = resetPoint.position;
        player.transform.rotation = resetPoint.rotation;
        player.GetComponent<CharacterController>().enabled = true;
    }

    public void SetNewCheckPoint(Transform point)
    {
        resetPoint = point;
    }
}
