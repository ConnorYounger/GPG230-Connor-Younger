using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class MultiplayerConnectManager : MonoBehaviourPunCallbacks
{
    public W8MainMenuManager menuManager;

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
        if(!hasConnected)
            PhotonNetwork.ConnectUsingSettings();
        else
            menuManager.ShowMultiplayerMenu();
    }

    public override void OnConnectedToMaster()
    {
        //multiplayerUI.SetActive(true);
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
        PhotonNetwork.CreateRoom(createInput.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("MultiplayerLevel1");
    }
}
