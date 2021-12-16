using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MultiplayerRoomItem : MonoBehaviour
{
    public TMP_Text roomName;
    private MultiplayerConnectManager lobbyManager;

    private void Start()
    {
        lobbyManager = GameObject.Find("MultiplayerConnectManager").GetComponent<MultiplayerConnectManager>();
    }

    public void SetRoomName(string name)
    {
        roomName.text = name;
    }

    public void JoinRoom()
    {
        lobbyManager.JoinCreatedRoom(roomName.text);
    }
}
