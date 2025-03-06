using UnityEngine;

public class PlayerManager : CharacterManager
{
    [HideInInspector] public PlayerAnimatorManager _playerAnimatorManager;
    [HideInInspector] public PlayerMovementManager _playerMovementManager;
    [HideInInspector] public PlayerStatsManager _playerStatsManager;
    [HideInInspector] public PlayerNetworkManager _playerNetworkManager;


    protected override void Awake() {
        base.Awake();

        // Find components
        _playerMovementManager = GetComponent<PlayerMovementManager>();
        _playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        _playerStatsManager = GetComponent<PlayerStatsManager>();
        _playerNetworkManager = GetComponent<PlayerNetworkManager>();



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

        if(!IsOwner) { return; }

        base.LateUpdate();

        PlayerCamera.Instance.HandleAllCameraActions();
    }

    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();
        if(IsOwner) { 
            
            // Give Reference to other scripts
            PlayerInputManager.Instance.Player = this;
            PlayerCamera.Instance.Player = this;        
        }
    }

    public override void SetCurrentStamina(float newStamina) {
        float OldStamina = CurrentStamina;
        base.SetCurrentStamina(newStamina);
        _playerStatsManager.ResetStaminaRegenTimer(OldStamina, newStamina);
    }


}