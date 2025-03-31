using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class PlayerManager : CharacterManager
{
    [Header("DEBUG MENU")]
    [SerializeField] bool _respawnCharacter = false;
    [SerializeField] bool _switchRightWeapon = false;
    [SerializeField] bool _switchLeftWeapon = false;

    [HideInInspector] public PlayerAnimatorManager PlayerAnimatorManager;
    [HideInInspector] public PlayerMovementManager PlayerMovementManager;
    [HideInInspector] public PlayerStatsManager PlayerStatsManager;
    [HideInInspector] public PlayerNetworkManager PlayerNetworkManager;
    [HideInInspector] public PlayerInventoryManager PlayerInventoryManager;
    [HideInInspector] public PlayerEquipmentManager PlayerEquipmentManager;
    [HideInInspector] public PlayerCombatManager PlayerCombatManager;



    protected override void Awake() {
        base.Awake();

        // Find components
        PlayerMovementManager = GetComponent<PlayerMovementManager>();
        PlayerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        PlayerStatsManager = GetComponent<PlayerStatsManager>();
        PlayerNetworkManager = GetComponent<PlayerNetworkManager>();
        PlayerInventoryManager = GetComponent<PlayerInventoryManager>();
        PlayerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        PlayerCombatManager = GetComponent<PlayerCombatManager>();
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
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;

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

        // Stats
        PlayerNetworkManager.CurrentHealth.OnValueChanged += PlayerNetworkManager.CheckHP;

        // Equipment
        PlayerNetworkManager.CurrentRightHandWeaponID.OnValueChanged += PlayerNetworkManager.OnCurrentRightHandWeaponIDChange;
        PlayerNetworkManager.CurrentLeftHandWeaponID.OnValueChanged += PlayerNetworkManager.OnCurrentLeftHandWeaponIDChange;
        PlayerNetworkManager.CurrentWeaponBeingUsed.OnValueChanged += PlayerNetworkManager.OnCurrentWeaponBeingUsedIDChange;

        // Load Stats When Joining
        if (IsOwner && !IsServer) {
            LoadDataFromCurrentCharacterData(ref WorldSaveGameManager.Instance.CurrentCharacterData);
        }
    }

    void OnClientConnectedCallback(ulong clientID) {
        // Keep List Of Active Players
        WorldGameSessionManager.Instance.AddPlayerToActivePlayersList(this);

        // TODO: Remove Later Debug
        PlayerNetworkManager.Vitality.Value = 10;
        PlayerNetworkManager.Endurance.Value = 10;

        // Check If Client & Not Host
        if (!IsServer && IsOwner) {
            foreach (PlayerManager player in WorldGameSessionManager.Instance.Players) {
                if (player != this) {
                    player.LoadOtherPlayerCharacterWhenJoiningServer();
                }
            }
        }
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

    public void LoadOtherPlayerCharacterWhenJoiningServer() {

        // Sync Weapons
        PlayerNetworkManager.OnCurrentRightHandWeaponIDChange(0, PlayerNetworkManager.CurrentRightHandWeaponID.Value);
        PlayerNetworkManager.OnCurrentLeftHandWeaponIDChange(0, PlayerNetworkManager.CurrentLeftHandWeaponID.Value);
    }

    // TODO: Delete Later
    void DebugMenu() {
        // Respawn
        if (_respawnCharacter) {
            _respawnCharacter = false;
            ReviveCharacter();
        }
        // Switch Right Hand Weapon
        if (_switchRightWeapon) { 
            _switchRightWeapon = false;
            PlayerEquipmentManager.SwitchRightWeapon();
        }
        // Switch Right Hand Weapon
        if (_switchLeftWeapon) {
            _switchLeftWeapon = false;
            PlayerEquipmentManager.SwitchLeftWeapon();
        }
    }
}