using UnityEngine;

public class PlayerManager : CharacterManager
{
    PlayerMovementManager _movement;

    protected override void Awake() {
        base.Awake();

        _movement = GetComponent<PlayerMovementManager>();

        PlayerCamera.Instance._player = this;
    }

    protected override void Update() {
        base.Update();

        _movement.HandleAllMovement();
    }

    protected override void LateUpdate() {
        base.LateUpdate();

        PlayerCamera.Instance.HandleAllCameraActions();
    }
}
