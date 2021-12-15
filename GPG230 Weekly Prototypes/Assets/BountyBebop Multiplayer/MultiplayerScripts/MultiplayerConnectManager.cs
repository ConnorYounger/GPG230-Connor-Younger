using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class MultiplayerConnectManager : MonoBehaviourPunCallbacks
{
    public W8MainMenuManager menuManager;

    public GameObject connectingUI;
    public GameObject connectingUI2;

    public TMP_InputField createInput;
    public TMP_InputField joinInput;

    public bool hasConnected;

    private void Start()
    {
        hasConnected = false;

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.LeaveLobby();
            PhotonNetwork.Disconnect();
        }
    }

    public void AtemptToConnectToServer()
    {
        if (!hasConnected)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
            connectingUI.SetActive(true);
        }
        else
            menuManager.ShowMultiplayerMenu();
    }

    public override void OnConnectedToMaster()
    {
        //multiplayerUI.SetActive(true);
        connectingUI.SetActive(false);
        menuManager.ShowMultiplayerMenu();

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        hasConnected = true;
        base.OnJoinedLobby();
    }

    // Join Room Buttons
    public void CreateRoom()
    {
        connectingUI2.SetActive(true);
        PhotonNetwork.CreateRoom(createInput.text);
    }

    public void JoinRoom()
    {
        connectingUI2.SetActive(true);
        Debug.Log("Count of rooms: " + PhotonNetwork.CountOfRooms);
        PhotonNetwork.JoinRoom(joinInput.text);
        Debug.Log("Is in room: " + PhotonNetwork.InRoom);
        Debug.Log("Current Room: " + PhotonNetwork.CurrentRoom);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("On Joined Room");
        PhotonNetwork.LoadLevel("MultiplayerLevel1");
        //connectingUI2.SetActive(false);
    }
}
