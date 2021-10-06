using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RoomManager : MonoBehaviour
{
    [System.Serializable]
    public struct room
    {
        [SerializeField] public GameObject roomEGO;
        [SerializeField] public CinemachineVirtualCamera roomCam;
        [SerializeField] public Transform[] roomEntrances;
    }

    [SerializeField] public room[] rooms;

    public int startingRoom;
    public int currentRoomIndex;
    public GameObject player;

    public W2Map mapManager;

    // Start is called before the first frame update
    void Start()
    {
        // Hide rooms at the start
        for (int i = 0; i < rooms.Length; i++)
        {
            if (i == startingRoom)
            {
                ShowRooms(startingRoom, true);
            }
            else
            {
                ShowRooms(i, false);
            }
        }

        currentRoomIndex = startingRoom;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TravelToNewRoom(int roomIndex, int roomEntranceIndex)
    {
        Debug.Log("Travel to: " + roomIndex + ", room entrance: " + roomEntranceIndex);

        currentRoomIndex = roomIndex;

        for(int i = 0; i < rooms.Length; i++)
        {
            if(i == roomIndex)
            {
                ShowRooms(i, true);

                // Teleport player
                player.GetComponent<W2Player>().navAgent.enabled = false;
                player.transform.position = rooms[i].roomEntrances[roomEntranceIndex].position;
                player.transform.rotation = rooms[i].roomEntrances[roomEntranceIndex].rotation;
                player.GetComponent<W2Player>().destinationPoint = player.transform.position;
                player.GetComponent<W2Player>().navAgent.enabled = true;

                StartCoroutine("StopPlayerMovement");
            }
            else
            {
                ShowRooms(i, false);
            }
        }

        if (mapManager)
        {
            mapManager.SetRoom(roomIndex);
        }
    }

    void ShowRooms(int index, bool value)
    {
        rooms[index].roomEGO.SetActive(value);
        rooms[index].roomCam.gameObject.SetActive(value);
    }

    IEnumerator StopPlayerMovement()
    {
        yield return new WaitForSeconds(0.1f);

        player.GetComponent<W2Player>().navAgent.enabled = false;
    }
}
