using UnityEngine;

public class PlayerManager : CharacterManager
{
    [HideInInspector] public PlayerAnimatorManager _playerAnimatorManager;
    [HideInInspector] public PlayerMovementManager _playerMovementManager;
    [HideInInspector] public PlayerStatsManager _playerStatsManager;


    protected override void Awake() {
        base.Awake();

        // Find components
        _playerMovementManager = GetComponent<PlayerMovementManager>();
        _playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        _playerStatsManager = GetComponent<PlayerStatsManager>();

        // Give Reference to other scripts
        PlayerInputManager.Instance.Player = this;
        PlayerCamera.Instance.Player = this;

        // Player Stamina Calculations
        MaxStamina = _playerStatsManager.CalculateStaminaBasedOnLevel(Endurance);
        SetCurrentStamina(_playerStatsManager.CalculateStaminaBasedOnLevel(Endurance));
        PlayerUIManager.Instance.PlayerUIHudManager.SetMaxStaminaValue(MaxStamina);
    }

    protected override void Update() {
        base.Update();

        if(!IsOwner) { return; }

        // Handle movement
        _playerMovementManager.HandleAllMovement();

        // Regenerate Stamina
        _playerStatsManager.RegenerateStamina();
    }

    protected override void LateUpdate() {
        base.LateUpdate();

        PlayerCamera.Instance.HandleAllCameraActions();
    }

    public override void SetCurrentStamina(float newStamina) {
        float OldStamina = CurrentStamina;
        base.SetCurrentStamina(newStamina);
        _playerStatsManager.ResetStaminaRegenTimer(OldStamina, newStamina);
    }


}