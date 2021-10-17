using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleConnections : MonoBehaviour
{
    public Transform[] connections;
    public bool[] isConnected;

    public W3Door door;
    public PhizDoor phisDoor;

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

            Debug.Log(connections[i] + " " + t);
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
        bool open = true;

        for (int i = 0; i < connections.Length; i++)
        {
            if (!isConnected[i])
            {
                open = false;
            }
        }

        if (open)
            OpenDoor();
        else
            CloseDoor();
    }

    void OpenDoor()
    {
        if(door)
            door.OpenDoor();

        if (phisDoor)
            phisDoor.TurnOff();

        Debug.Log("Open Door");
    }

    void CloseDoor()
    {
        if(door)
            door.CloseDoor();

        if (phisDoor)
            phisDoor.TurnOn();

        Debug.Log("Close Door");
    }
}
