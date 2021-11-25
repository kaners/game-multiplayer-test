using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform[] spawnPoints;

    public static event Action<PlayerController> OnPlayerSpawned = delegate { };

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        PlayerController myPlayer = null;
        Transform spawnPoint = spawnPoints[0];

        if (PhotonNetwork.InRoom)
        {
            int spawnIndex = (int) Mathf.Repeat(PhotonNetwork.CurrentRoom.PlayerCount - 1, spawnPoints.Length);
            spawnPoint = spawnPoints[spawnIndex];
            myPlayer = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation, 0).GetComponent<PlayerController>();
        }
        else
        {
            myPlayer = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation).GetComponent<PlayerController>();
        }

        //Do anything with myPlayer

        OnPlayerSpawned(myPlayer);
    }
}