using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : CharacterManager
{
    [Header("DEBUG MENU")]
    [SerializeField] bool _respawnCharacter = false;

    [HideInInspector] public PlayerAnimatorManager PlayerAnimatorManager;
    [HideInInspector] public PlayerMovementManager PlayerMovementManager;
    [HideInInspector] public PlayerStatsManager PlayerStatsManager;
    [HideInInspector] public PlayerNetworkManager PlayerNetworkManager;
    [HideInInspector] public PlayerInventoryManager PlayerInventoryManager;


    protected override void Awake() {
        base.Awake();

        // Find components
        PlayerMovementManager = GetComponent<PlayerMovementManager>();
        PlayerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        PlayerStatsManager = GetComponent<PlayerStatsManager>();
        PlayerNetworkManager = GetComponent<PlayerNetworkManager>();
        PlayerInventoryManager = GetComponent<PlayerInventoryManager>();

    }

    protected override void Update() {
        base.Update();

        if(!IsOwner) { return; }

        // Handle movement
        PlayerMovementManager.HandleAllMovement();

        // Regenerate Stamina
        PlayerStatsManager.RegenerateStamina();

        DebugMenu();
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
            WorldSaveGameManager.Instance.Player = this;

            // Update Total Amount Of (Health/Stamina) With Level (Vitality/Endurance)
            PlayerNetworkManager.Vitality.OnValueChanged += PlayerNetworkManager.SetNewMaxHealthValue;
            PlayerNetworkManager.Endurance.OnValueChanged += PlayerNetworkManager.SetNewMaxStaminaValue;

            // Updates HUD UI When Value Change (HP/Stamina)
            PlayerNetworkManager.CurrentHealth.OnValueChanged += PlayerUIManager.Instance.PlayerUIHudManager.SetNewHealthValue;
            PlayerNetworkManager.CurrentStamina.OnValueChanged += PlayerUIManager.Instance.PlayerUIHudManager.SetNewStaminaValue;
            PlayerNetworkManager.CurrentStamina.OnValueChanged += PlayerStatsManager.ResetStaminaRegenTimer;

        }

        PlayerNetworkManager.CurrentHealth.OnValueChanged += PlayerNetworkManager.CheckHP;
    }

    public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false) {

        if (IsOwner) {
            PlayerUIManager.Instance.PlayerUIPopUpManager.SendYouDiedPopUp();
        }

        return base.ProcessDeathEvent(manuallySelectDeathAnimation);

        // TODO: Do Respawning

    }

    public override void ReviveCharacter() {
        base.ReviveCharacter();

        if (IsOwner) {
            PlayerNetworkManager.CurrentHealth.Value = PlayerNetworkManager.MaxHealth.Value;
            PlayerNetworkManager.CurrentStamina.Value = PlayerNetworkManager.MaxStamina.Value;
            // Restore Mana

            // Playe Revive Effect
            PlayerAnimatorManager.PlayTargetActionAnimation("Empty", false);
        }
    }

    public void SaveDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData) {
        // Give Character Scene
        currentCharacterData.SceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Give Character Name
        currentCharacterData.CharacterName = PlayerNetworkManager.CharacterName.Value.ToString();

        // Give Character Position
        currentCharacterData.xPosition = transform.position.x;
        currentCharacterData.yPosition = transform.position.y;
        currentCharacterData.zPosition = transform.position.z;

        // Give Character Stats
        currentCharacterData.Vitality = PlayerNetworkManager.Vitality.Value;
        currentCharacterData.Endurance = PlayerNetworkManager.Endurance.Value;

        // Give Character Resourcess
        currentCharacterData.CurrentHealth = PlayerNetworkManager.CurrentHealth.Value;
        currentCharacterData.CurrentStamina = PlayerNetworkManager.CurrentStamina.Value;
    }

    public void LoadDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData) {
        // Get Character Name
        PlayerNetworkManager.CharacterName.Value = currentCharacterData.CharacterName;

        // Get Character Position
        Vector3 position = new Vector3(currentCharacterData.xPosition, currentCharacterData.yPosition, currentCharacterData.zPosition);
        transform.position = position;

        // Give Character Stats
        PlayerNetworkManager.Vitality.Value = currentCharacterData.Vitality;
        PlayerNetworkManager.Endurance.Value = currentCharacterData.Endurance;

        // Get Character Resourcess
        PlayerNetworkManager.MaxHealth.Value = PlayerStatsManager.CalculateHealthBasedOnLevel(currentCharacterData.Vitality);
        PlayerNetworkManager.MaxStamina.Value = PlayerStatsManager.CalculateStaminaBasedOnLevel(currentCharacterData.Endurance);
        PlayerNetworkManager.CurrentHealth.Value = currentCharacterData.CurrentHealth;
        PlayerNetworkManager.CurrentStamina.Value = currentCharacterData.CurrentStamina;
        PlayerUIManager.Instance.PlayerUIHudManager.SetMaxStaminaValue(PlayerNetworkManager.MaxStamina.Value);
    }

    

    // TODO: Delete Later
    void DebugMenu() {
        if (_respawnCharacter) {
            _respawnCharacter = false;
            ReviveCharacter();
        }
    }
}