using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleConnections : MonoBehaviour
{
    public Transform[] connections;
    public bool[] isConnected;

    public W3Door door;

    void Start()
    {
        SetUpConnections();
    }

    void Update()
    {
        
    }

    void SetUpConnections()
    {
        connections = new Transform[transform.childCount];
        isConnected = new bool[transform.childCount];

        for(int i = 0; i < connections.Length; i++)
        {
            connections[i] = transform.GetChild(i);
        }
    }

    public void Activate(Transform t)
    {
        for (int i = 0; i < connections.Length; i++)
        {
            if(connections[i] == t)
            {
                isConnected[i] = true;
            }
        }

        CheckForFullActivation();
    }

    public void Deactivate(Transform t)
    {
        for (int i = 0; i < connections.Length; i++)
        {
            if (connections[i] == t)
            {
                isConnected[i] = false;
            }
        }

        CheckForFullActivation();
    }

    void CheckForFullActivation()
    {
        for (int i = 0; i < connections.Length; i++)
        {
            bool open = false;

            if (connections[i])
            {
                open = true;
            }

            if(open)
                OpenDoor();
            else
                CloseDoor();
        }
    }

    void OpenDoor()
    {
        door.OpenDoor();

        Debug.Log("Open Door");
    }

    void CloseDoor()
    {
        door.CloseDoor();

        Debug.Log("Close Door");
    }
}
