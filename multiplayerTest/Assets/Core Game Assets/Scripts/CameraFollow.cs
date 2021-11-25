using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset = new Vector3(0, 3, -2);

    void Awake()
    {
        SpawnManager.OnPlayerSpawned += OnPlayerSpawned;
    }
    void OnDestroy()
    {
        SpawnManager.OnPlayerSpawned -= OnPlayerSpawned;
    }

    void OnPlayerSpawned(PlayerController player)
    {
        if (!player.photonView.IsMineOrOffline())
            return;

        target = player.transform;
    }

    void LateUpdate()
    {
        if (!target)
            return;

        transform.position = target.position + offset;
    }
}