using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MultiplayerScenarioManager : MonoBehaviour
{
    public static PhotonView playerPhotonView;

    public GameObject playerPrefab;

    public Transform spawnPointsParent;
    public Transform[] spawnPoints;

    void Start()
    {
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


}
