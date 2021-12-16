using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class MultiplayerScenarioManager : MonoBehaviourPunCallbacks
{
    public static PhotonView playerPhotonView;
    public PhotonView photonView;

    public GameObject playerPrefab;

    public Transform spawnPointsParent;
    public Transform[] spawnPoints;
    public GameObject winUI;
    public Transform winTransformGroup;

    public GameObject leaderBoardPrefab;
    public Transform leaderBoardGroup;
    public List<MultiplayerScenarioPlayerStats> players;
    public List<LeaderBoardStat> leaderBoardStats;

    public int killsToWin = 15;
    public int leaveGameTime = 3;
    private int leaveCounter;
    public TMP_Text counterText;

    public TMP_Text winCurrencyText;

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
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
    }

    void Update()
    {
        
    }

    [PunRPC]
    public void PlayerGotKilled(int playerWhoDied, int playerWhoKilled)
    {
        Debug.Log("Player" + PhotonView.Find(playerWhoDied) + " was killed by Player" + PhotonView.Find(playerWhoKilled));

        for(int i = 0; i < players.Count; i++)
        {
            if(players[i].player == PhotonView.Find(playerWhoDied))
            {
                players[i].deaths++;
            }

            if (players[i].player == PhotonView.Find(playerWhoKilled))
            {
                players[i].kills++;

                if (PhotonNetwork.IsMasterClient)
                {
                    if (players[i].kills >= killsToWin)
                    {
                        AdminPlayerWin();
                    }
                }
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
            if (stat.player.player.ViewID == viewID)
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
        if (players.Count > 0 && leaderBoardStats.Count > 0)
        {
            for (int i = 0; i < players.Count; i++)
            {
                leaderBoardStats[i].playerName.text = "Player" + players[i].player.ViewID;
                leaderBoardStats[i].killsText.text = players[i].kills.ToString();
                leaderBoardStats[i].deathsText.text = players[i].deaths.ToString();
            }
        }

        // Order by kills
    }

    void AdminPlayerWin()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            MultiplayerScenarioPlayerStats firstPlayer = new MultiplayerScenarioPlayerStats();
            MultiplayerScenarioPlayerStats secondPlayer = new MultiplayerScenarioPlayerStats();
            MultiplayerScenarioPlayerStats thirdPlayer = new MultiplayerScenarioPlayerStats();

            int firstPlayerID = 0;
            int secondPlayerID = 0;
            int thirdPlayerID = 0;

            foreach (MultiplayerScenarioPlayerStats player in players)
            {
                if (player.kills > firstPlayer.kills)
                {
                    firstPlayer = player;
                }
                else if (player.kills > secondPlayer.kills)
                {
                    secondPlayer = player;
                }
                else if (player.kills > thirdPlayer.kills)
                {
                    thirdPlayer = player;
                }
            }

            //players.Sort(SortByKills);

            //if (players[0] != null)
            //    firstPlayerID = players[0].player.ViewID;
            //if (players[1] != null)
            //    secondPlayerID = players[1].player.ViewID;
            //if (players[2] != null)
            //    thirdPlayerID = players[2].player.ViewID;

            if (firstPlayer != null)
                firstPlayerID = firstPlayer.player.ViewID;
            if (secondPlayer != null && secondPlayer.player != null)
                secondPlayerID = secondPlayer.player.ViewID;
            if (thirdPlayer != null && thirdPlayer.player != null)
                thirdPlayerID = thirdPlayer.player.ViewID;

            photonView.RPC("PlayerWin", RpcTarget.AllBuffered, firstPlayerID, secondPlayerID, thirdPlayerID);
        }
    }

    int SortByKills(MultiplayerScenarioPlayerStats p1, MultiplayerScenarioPlayerStats p2)
    {
        return p1.kills.CompareTo(p2.kills);
    }

    [PunRPC]
    void PlayerWin(int firstPlayer, int secondPlayer, int thirdPlayer)
    {
        foreach(MultiplayerScenarioPlayerStats player in players)
        {
            if(player.player.ViewID == firstPlayer)
            {
                CreateNewWinningLeaderBoard(player, 0);

                if (player.player.IsMine) 
                {
                    W8SaveData.AddScenarioCurrency(4000);

                    ShowCurrencyUI(4000);
                }
            }
            else if (secondPlayer > 0 && player.player.ViewID == secondPlayer)
            {
                CreateNewWinningLeaderBoard(player, 1);

                if (player.player.IsMine)
                {
                    W8SaveData.AddScenarioCurrency(3000);

                    ShowCurrencyUI(3000);
                }
            }
            else if (thirdPlayer > 0 && player.player.ViewID == thirdPlayer)
            {
                CreateNewWinningLeaderBoard(player, 2);

                if (player.player.IsMine)
                {
                    W8SaveData.AddScenarioCurrency(2000);

                    ShowCurrencyUI(2000);
                }
            }
            else
            {
                if (player.player.IsMine)
                {
                    W8SaveData.AddScenarioCurrency(1000);

                    ShowCurrencyUI(1000);
                }
            }
        }

        leaveCounter = leaveGameTime;
        counterText.text = leaveCounter.ToString();

        winUI.SetActive(true);
        StopCoroutine("LeaveGameCountDown");
        StartCoroutine("LeaveGameCountDown");

        SaveSystem.SaveStats(W8SaveData.w8SaveData);
    }

    void ShowCurrencyUI(int amount)
    {
        winCurrencyText.text = amount.ToString();
    }

    LeaderBoardStat CreateNewWinningLeaderBoard(MultiplayerScenarioPlayerStats player, int pos)
    {
        GameObject newLeaderObject = Instantiate(leaderBoardPrefab, winTransformGroup.position, Quaternion.identity);
        newLeaderObject.transform.parent = winTransformGroup;

        LeaderBoardStat newLeaderBoard = newLeaderObject.GetComponent<LeaderBoardStat>();
        newLeaderBoard.player = player;
        leaderBoardStats.Add(newLeaderBoard);

        if (pos == 0)
        {
            newLeaderObject.transform.parent.SetAsFirstSibling();
            newLeaderBoard.positionText.text = "1";
        }
        else if (pos == 2)
        {
            newLeaderObject.transform.parent.SetAsLastSibling();
            newLeaderBoard.positionText.text = "2";
        }
        else
        {
            newLeaderBoard.positionText.text = "3";
        }

        newLeaderBoard.playerName.text = "Player" + player.player.ViewID.ToString();
        newLeaderBoard.killsText.text = player.kills.ToString();
        newLeaderBoard.deathsText.text = player.deaths.ToString();

        if (PhotonView.Find(newLeaderBoard.player.player.ViewID).IsMine)
        {
            newLeaderBoard.playerName.color = Color.green;
        }

        return newLeaderBoard;
    }

    IEnumerator LeaveGameCountDown()
    {
        yield return new WaitForSeconds(1);

        leaveCounter--;
        counterText.text = "Retunring in: " + leaveCounter.ToString() + "s";

        if(leaveCounter <= 0)
        {
            LeaveGame();
        }
        else
        {
            StartCoroutine("LeaveGameCountDown");
        }
    }

    void LeaveGame()
    {
        Debug.Log("Leave Game");
        SceneManager.LoadScene("Week8MainMenu");
    }
}
