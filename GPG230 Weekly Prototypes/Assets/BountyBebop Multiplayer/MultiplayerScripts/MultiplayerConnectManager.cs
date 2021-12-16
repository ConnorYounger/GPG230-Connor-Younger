using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class MultiplayerConnectManager : MonoBehaviourPunCallbacks
{
    public W8MainMenuManager menuManager;

    public GameObject connectingUI;
    public GameObject connectingUI2;

    public GameObject createRoomUI;

    public TMP_InputField createInput;
    public TMP_InputField joinInput;

    public MultiplayerRoomItem roomItemPrefab;
    public List<MultiplayerRoomItem> roomList = new List<MultiplayerRoomItem>();
    public Transform layerGroup;

    public bool hasConnected;

    [Header("LevelSelect")]
    public TMP_Text levelText;
    private string[] levelNames = { "Space Station Alpha", "Space Station Bravo", "Dyson Star", "Space Colony" };
    private string[] levelSceneNames = { "MultiplayerLevel1", "MultiplayerLevel2", "MultiplayerLevel3", "MultiplayerLevel4" };
    private int currentLevelIndex;

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
            PhotonNetwork.ConnectToRegion("au");
            connectingUI.SetActive(true);
        }
        else
            menuManager.ShowMultiplayerMenu();
    }

    public void ShowCreateRoomUI()
    {
        createRoomUI.SetActive(true);
    }

    public void HideCreateRoomUI()
    {
        createRoomUI.SetActive(false);
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

    public void SwitchCurrentLevel(int i)
    {
        currentLevelIndex += i;

        if(currentLevelIndex >= levelNames.Length)
        {
            currentLevelIndex = 0;
        }
        else if (currentLevelIndex <= 0)
        {
            currentLevelIndex = levelNames.Length - 1;
        }

        levelText.text = levelNames[currentLevelIndex];
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
        PhotonNetwork.LoadLevel(levelSceneNames[currentLevelIndex]);
        //connectingUI2.SetActive(false);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateRoomList(roomList);
    }

    void UpdateRoomList(List<RoomInfo> list)
    {
        foreach(MultiplayerRoomItem item in roomList)
        {
            Destroy(item.gameObject);
        }

        roomList.Clear();

        foreach(RoomInfo room in list)
        {
            MultiplayerRoomItem newItem = Instantiate(roomItemPrefab, layerGroup);

            newItem.SetRoomName(room.Name);
            roomList.Add(newItem);
        }
    }

    public void JoinCreatedRoom(string roomName)
    {
        connectingUI2.SetActive(true);
        PhotonNetwork.JoinRoom(roomName);
    }
}
