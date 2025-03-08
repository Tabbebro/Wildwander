using UnityEngine;

public class PlayerManager : CharacterManager
{
    [HideInInspector] public PlayerAnimatorManager PlayerAnimatorManager;
    [HideInInspector] public PlayerMovementManager PlayerMovementManager;
    [HideInInspector] public PlayerStatsManager PlayerStatsManager;
    [HideInInspector] public PlayerNetworkManager PlayerNetworkManager;


    protected override void Awake() {
        base.Awake();

        // Find components
        PlayerMovementManager = GetComponent<PlayerMovementManager>();
        PlayerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        PlayerStatsManager = GetComponent<PlayerStatsManager>();
        PlayerNetworkManager = GetComponent<PlayerNetworkManager>();

    }

    protected override void Update() {
        base.Update();

        if(!IsOwner) { return; }

        // Handle movement
        PlayerMovementManager.HandleAllMovement();

        // Regenerate Stamina
        PlayerStatsManager.RegenerateStamina();
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

            PlayerNetworkManager.CurrentStamina.OnValueChanged += PlayerUIManager.Instance.PlayerUIHudManager.SetNewStaminaValue;
            PlayerNetworkManager.CurrentStamina.OnValueChanged += PlayerStatsManager.ResetStaminaRegenTimer;

            PlayerNetworkManager.MaxStamina.Value = PlayerStatsManager.CalculateStaminaBasedOnLevel(PlayerNetworkManager.Endurance.Value);
            PlayerNetworkManager.CurrentStamina.Value = PlayerStatsManager.CalculateStaminaBasedOnLevel(PlayerNetworkManager.Endurance.Value);
            PlayerUIManager.Instance.PlayerUIHudManager.SetMaxStaminaValue(PlayerNetworkManager.MaxStamina.Value);
        }
    }

    public void SaveDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData) {
        // Give Character Name
        currentCharacterData.CharacterName = PlayerNetworkManager.CharacterName.Value.ToString();

        // Give Character Position
        currentCharacterData.xPosition = transform.position.x;
        currentCharacterData.yPosition = transform.position.y;
        currentCharacterData.zPosition = transform.position.z;
    }

    public void LoadDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData) {
        // Get Character Name
        PlayerNetworkManager.CharacterName.Value = currentCharacterData.CharacterName;

        // Get Character Position
        Vector3 position = new Vector3(currentCharacterData.xPosition, currentCharacterData.yPosition, currentCharacterData.zPosition);
        transform.position = position;
    }
}