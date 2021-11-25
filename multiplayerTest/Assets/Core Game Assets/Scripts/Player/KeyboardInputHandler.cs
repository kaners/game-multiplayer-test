using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInputHandler : PlayerInputHandler
{
    public override void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Quaternion camRot = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
        Vector3 inputVector = camRot * new Vector3(h, 0, v);

        FeedCharacter(inputVector);
    }
}