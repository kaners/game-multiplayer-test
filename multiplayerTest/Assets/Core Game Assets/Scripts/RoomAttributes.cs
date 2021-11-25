using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomAttributes : MonoBehaviour
{
    public static List<RoomAttributes> roomAttributeEntries = new List<RoomAttributes>();

    public Image thumbnailImage;
    public Text roomNameText;
    public Text playerCountText;

    RoomInfo room;

    void Awake()
    {
        roomAttributeEntries.Add(this);
    }
    void OnDestroy()
    {
        roomAttributeEntries.Remove(this);
    }

    public void UpdateRoomInfo(RoomInfo room)
    {
        this.room = room;

        LoadRoomVisualData();
    }

    void LoadRoomVisualData()
    {
        if (room == null)
            return;

        roomNameText.text = room.Name;
        playerCountText.text = $"Players ({room.PlayerCount}/{room.MaxPlayers})";
        thumbnailImage.sprite = RoomDataHolder.Instance.roomImages[(byte)room.CustomProperties["lvl"]];
    }

    public void BUTTON_Join()
    {
        OnlineManager.Instance.JoinRoom(room);
    }
}