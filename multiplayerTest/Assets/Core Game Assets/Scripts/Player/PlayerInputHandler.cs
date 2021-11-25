using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerInputHandler : MonoBehaviour
{
    protected PlayerController controller;

    public virtual void Awake()
    {
        controller = GetComponent<PlayerController>();

        if (!controller.photonView.IsMineOrOffline())
            enabled = false;
    }
    public virtual void Update()
    {
        
    }
    protected void FeedCharacter(Vector3 direction)
    {
        controller.SetInputVector(direction);
    }
}