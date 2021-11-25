using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Room Holder", menuName = "Scriptables/Create Room Holder")]
public class RoomDataHolder : ScriptableObject
{
    static RoomDataHolder instance;
    public static RoomDataHolder Instance => instance ?? (instance = Resources.Load<RoomDataHolder>("Room Holder"));

    public Sprite[] roomImages;
}