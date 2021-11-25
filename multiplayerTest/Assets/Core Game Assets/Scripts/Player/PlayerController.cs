using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviourPun
{
    CharacterController controller;
    Vector3 direction;

    [SerializeField] float maxSpeed = 10;
    [SerializeField] float directionSmoothing = 3;
    Vector3 inputVector;
    Vector3 motion;

    public float GetSpeed() => motion.magnitude;
    public float GetSpeedNormalized() => GetSpeed() / maxSpeed;
    public Vector3 GetDirection() => direction;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (!photonView.IsMineOrOffline())
            return;
    }

    void Update()
    {
        HandleInput();
        HandleRotation();
        ApplyMovementToController();
    }

    void HandleInput()
    {
        if (!controller)
            return;

        direction = Vector3.Lerp(direction, inputVector.normalized, directionSmoothing * Time.deltaTime);
    }
    void HandleRotation()
    {
        if (!controller)
            return;

        if (direction.normalized != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction.normalized);
    }
    void ApplyMovementToController()
    {
        if (!controller)
            return;

        float magnitude = Mathf.Clamp01(inputVector.magnitude);
        motion = direction * magnitude * maxSpeed;
        controller.Move(motion * Time.deltaTime);
    }

    public void SetInputVector(Vector3 newInput)
    {
        inputVector = newInput;
    }
}