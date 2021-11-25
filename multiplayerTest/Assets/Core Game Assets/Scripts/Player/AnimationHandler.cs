using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class AnimationHandler : MonoBehaviour
{
    PhotonView photonView;
    [SerializeField] Animator anim;
    PlayerController controller;

    float speedNormalized;

    void Awake()
    {
        controller = GetComponent<PlayerController>();
        photonView = GetComponentInParent<PhotonView>();
    }

    void Update()
    {
        if (!photonView.IsMineOrOffline())
            return;

        CacheControllerValues();
        FeedAnimatorParameters();
    }

    void CacheControllerValues()
    {
        speedNormalized = controller.GetSpeedNormalized();
    }
    void FeedAnimatorParameters()
    {
        anim.SetFloat("Speed", speedNormalized);
    }
}