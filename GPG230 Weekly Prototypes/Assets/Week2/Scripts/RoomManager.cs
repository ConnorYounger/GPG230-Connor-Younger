using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RoomManager : MonoBehaviour
{
    [System.Serializable]
    public struct room
    {
        [SerializeField] public CinemachineVirtualCamera roomCam;
        [SerializeField] public Transform[] roomEntrances;
    }

    [SerializeField] public room[] rooms;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TravelToNewRoom(int roomIndex, int roomEntranceIndex)
    {
        Debug.Log("Travel to: " + roomIndex + ", room entrance: " + roomEntranceIndex);
    }
}
