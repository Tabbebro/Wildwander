using UnityEngine;

public class PlayerManager : CharacterManager
{
    [HideInInspector] public PlayerAnimatorManager _playerAnimatorManager;
    [HideInInspector] public PlayerMovementManager _playerMovementManager;

    // Flags
    [Header("Flags (Player Manager)")]
    public bool isSprinting = false;

    protected override void Awake() {
        base.Awake();

        _playerMovementManager = GetComponent<PlayerMovementManager>();
        _playerAnimatorManager = GetComponent<PlayerAnimatorManager>();

        PlayerInputManager.Instance.Player = this;
        PlayerCamera.Instance.Player = this;
    }

    protected override void Update() {
        base.Update();

        _playerMovementManager.HandleAllMovement();
    }

    protected override void LateUpdate() {
        base.LateUpdate();

        PlayerCamera.Instance.HandleAllCameraActions();
    }
}
