using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class OnlineManager : MonoBehaviourPunCallbacks
{
    public static OnlineManager Instance;

    public RoomAttributes roomEntryPrefab;
    public Transform roomListParent;

    [Header("Testing")]
    public bool debugConnectionStatus;

    Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();

    static string[] customRoomPropertiesForLobby =
    {
        "lvl",
    };

    void Awake()
    {
        Instance = this;
        //Prevents the bug: Other players don't exist when joining a room
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        //SpawnManager.Instance.Spawn();

        SceneManager.LoadScene(1);
    }
    public void JoinRoom(RoomInfo room)
    {
        PhotonNetwork.JoinRoom(room.Name);
    }
    public void CreateRoom(string roomName, byte levelIndex, byte maxPlayers)
    {
        if (string.IsNullOrEmpty(roomName) || string.IsNullOrWhiteSpace(roomName))
            return;

        RoomOptions options = new RoomOptions();
        Hashtable customProperties = new Hashtable();

        customProperties.Add("lvl", levelIndex);

        options.MaxPlayers = maxPlayers;
        options.CustomRoomPropertiesForLobby = customRoomPropertiesForLobby;
        options.CustomRoomProperties = customProperties;

        PhotonNetwork.JoinOrCreateRoom(roomName, options, null);
        //PhotonNetwork.JoinRandomOrCreateRoom(null);
    }

    public void BUTTON_RefreshRooms() => RefreshRooms();
    void RefreshRooms()
    {
        DeleteExistingRooms();

        int roomListCount = cachedRoomList.Count;
        for (int i = 0; i < roomListCount; i++)
        {
            RoomInfo room = cachedRoomList.Values.ToList()[i];
            RoomAttributes roomUIEntry = Instantiate(roomEntryPrefab, roomListParent);
            roomUIEntry.UpdateRoomInfo(room);
        }
    }

    void DeleteExistingRooms()
    {
        int roomAttributeCount = RoomAttributes.roomAttributeEntries.Count;
        for (int i = 0; i < roomAttributeCount; i++)
            Destroy(RoomAttributes.roomAttributeEntries[i].gameObject);
    }

    #region ILobbyCallbacks
    public override void OnJoinedLobby() => RefreshRooms();
    public override void OnLeftLobby() => RefreshRooms();
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Room List Updated");

        for (int i = 0; i < roomList.Count; i++)
        {
            RoomInfo info = roomList[i];
            if (info.RemovedFromList)
            {
                cachedRoomList.Remove(info.Name);
            }
            else
            {
                cachedRoomList[info.Name] = info;
            }
        }
    }
    public override void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics) { }
    #endregion

    #region Testing
    void OnGUI()
    {
        DebugCononectionStatus();
    }
    void DebugCononectionStatus()
    {
        if (!debugConnectionStatus)
            return;

        GUISkin s = new GUISkin();
        s.label.fontSize = 30;
        s.label.normal.textColor = Color.white;

        GUILayout.Label(PhotonNetwork.NetworkClientState.ToString(), s.label);
    }
    #endregion
}