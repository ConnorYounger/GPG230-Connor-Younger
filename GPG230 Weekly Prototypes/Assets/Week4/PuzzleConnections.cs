using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleConnections : MonoBehaviour
{
    public Transform[] connections;
    public bool[] isConnected;

    public W3Door door;
    public PhizDoor phisDoor;
    public PhizDoor[] extraPhisDoor;

    private List<Transform> addedConnections = new List<Transform>();

    void Start()
    {
        SetUpConnections();
        StartCoroutine("AddExtraConnections");
    }

    void Update()
    {
        
    }

    public void AddConnection(Transform t)
    {
        addedConnections.Add(t);
    }

    IEnumerator AddExtraConnections()
    {
        yield return new WaitForSeconds(1);

        if (addedConnections.Count > 0)
        {
            Debug.Log("Add extra connections");

            if (connections.Length > 0) 
            {
                foreach (Transform t in connections)
                {
                    addedConnections.Add(t);
                }
            }

            connections = new Transform[addedConnections.Count];
            isConnected = new bool[connections.Length];

            for (int i = 0; i < connections.Length; i++)
            {
                connections[i] = addedConnections[i];
            }

            addedConnections.Clear();
        }

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
            phisDoor.TurnOff(gameObject);

        if(extraPhisDoor.Length > 0)
        {
            foreach(PhizDoor p in extraPhisDoor)
            {
                p.TurnOff(gameObject);
            }
        }

        Debug.Log("Open Door");
    }

    void CloseDoor()
    {
        if(door)
            door.CloseDoor();

        if (phisDoor)
            phisDoor.TurnOn(gameObject);

        if (extraPhisDoor.Length > 0)
        {
            foreach (PhizDoor p in extraPhisDoor)
            {
                p.TurnOn(gameObject);
            }
        }

        Debug.Log("Close Door");
    }
}
