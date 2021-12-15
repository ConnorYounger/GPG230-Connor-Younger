using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MultiplayerScenarioManager : MonoBehaviourPunCallbacks
{
    public static PhotonView playerPhotonView;
    public PhotonView photonView;

    public GameObject playerPrefab;

    public Transform spawnPointsParent;
    public Transform[] spawnPoints;

    public GameObject leaderBoardPrefab;
    public Transform leaderBoardGroup;
    public List<MultiplayerScenarioPlayerStats> players;
    public List<LeaderBoardStat> leaderBoardStats; 

    void Start()
    {
        photonView = gameObject.GetComponent<PhotonView>();

        players = new List<MultiplayerScenarioPlayerStats>();

        spawnPoints = new Transform[spawnPointsParent.childCount];

        for(int i = 0; i < spawnPoints.Length; i++)
        {
            spawnPoints[i] = spawnPointsParent.GetChild(i);
        }

        int spawnIndex = PhotonNetwork.CurrentRoom.PlayerCount;
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoints[spawnIndex].position, Quaternion.identity);
    }

    void Update()
    {
        
    }

    [PunRPC]
    public void PlayerGotKilled(int playerWhoDied, int playerWhoKilled)
    {
        for(int i = 0; i < players.Count; i++)
        {
            if(players[i].player == PhotonView.Find(playerWhoDied))
            {
                players[i].deaths++;
            }

            if (players[i].player == PhotonView.Find(playerWhoKilled))
            {
                players[i].kills++;
            }
        }

        UpdateLeaderBoard();
    }

    [PunRPC]
    public void NewPlayerJoined(int viewID)
    {
        MultiplayerScenarioPlayerStats newStats = new MultiplayerScenarioPlayerStats();
        newStats.player = PhotonView.Find(viewID);
        players.Add(newStats);

        GameObject newLeaderObject = Instantiate(leaderBoardPrefab, leaderBoardGroup.position, Quaternion.identity);
        newLeaderObject.transform.parent = leaderBoardGroup;

        LeaderBoardStat newLeaderBoard = newLeaderObject.GetComponent<LeaderBoardStat>();
        newLeaderBoard.player = newStats;
        leaderBoardStats.Add(newLeaderBoard);

        if (PhotonView.Find(viewID).IsMine)
        {
            newLeaderBoard.playerName.color = Color.green;
        }

        UpdateLeaderBoard();
    }

    [PunRPC]
    public void PlayerLeft(int viewID)
    {
        GameObject leaderBoardToRemove = null;
        LeaderBoardStat leaderBoardStatToRemove = null;
        MultiplayerScenarioPlayerStats playerToRemove = null;

        foreach (LeaderBoardStat stat in leaderBoardStats)
        {
            if (stat.player.player == PhotonView.Find(viewID))
            {
                leaderBoardToRemove = stat.gameObject;
                leaderBoardStatToRemove = stat;
            }
        }

        foreach (MultiplayerScenarioPlayerStats stat in players)
        {
            if(stat.player == PhotonView.Find(viewID))
            {
                playerToRemove = stat;
            }
        }

        if(playerToRemove != null)
        {
            players.Remove(playerToRemove);
        }

        if(leaderBoardToRemove != null)
        {
            Destroy(leaderBoardToRemove);
        }
        
        if(leaderBoardStatToRemove != null)
        {
            leaderBoardStats.Remove(leaderBoardStatToRemove);
        }

        UpdateLeaderBoard();
    }

    public void UpdateLeaderBoard()
    {
        for(int i = 0; i < players.Count; i++)
        {
            leaderBoardStats[i].playerName.text = "Player" + players[i].player.ViewID;
            leaderBoardStats[i].killsText.text = players[i].kills.ToString();
            leaderBoardStats[i].deathsText.text = players[i].deaths.ToString();
        }

        // Order by kills
    }
}
