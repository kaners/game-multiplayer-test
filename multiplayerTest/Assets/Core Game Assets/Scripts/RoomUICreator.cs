using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class RoomUICreator : MonoBehaviour
{
    public static RoomUICreator Instance;

    public InputField roomNameField;
    public Image thumbnail;
    public Text maxPlayersText;

    [SerializeField] int minPlayerCount = 2;
    [SerializeField] int maxPlayerCount = 8;

    int maxPlayers = 2;
    int levelIndex = 0;

    void Awake()
    {
        Instance = this;

        maxPlayers = minPlayerCount;
    }

    void ChangePlayerCount(int change)
    {
        maxPlayers += change;

        if (maxPlayers >= maxPlayerCount)
            maxPlayers = maxPlayerCount;
        if (maxPlayers <= minPlayerCount)
            maxPlayers = minPlayerCount;

        maxPlayersText.text = $"Max Players ({maxPlayers})";
    }
    void ChangeLevelIndex(int change)
    {
        Sprite[] roomImages = RoomDataHolder.Instance.roomImages;
        levelIndex = (int)Mathf.Repeat(levelIndex + change, roomImages.Length);

        thumbnail.sprite = roomImages[levelIndex];
    }
    void CreateRoom()
    {
        OnlineManager.Instance.CreateRoom(roomNameField.text, (byte)levelIndex, (byte)maxPlayers);
    }

    #region Buttons
    public void BUTTON_ChangePlayerCount(int change) => ChangePlayerCount(change);
    public void BUTTON_ChangeLevelIndex(int change) => ChangeLevelIndex(change);
    public void BUTTON_CreateRoom() => CreateRoom();
    #endregion
}