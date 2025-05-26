using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class PlayerManager : CharacterManager
{
    [HideInInspector] public PlayerAnimatorManager PlayerAnimatorManager;
    [HideInInspector] public PlayerMovementManager PlayerMovementManager;
    [HideInInspector] public PlayerStatsManager PlayerStatsManager;
    [HideInInspector] public PlayerNetworkManager PlayerNetworkManager;
    [HideInInspector] public PlayerInventoryManager PlayerInventoryManager;
    [HideInInspector] public PlayerEquipmentManager PlayerEquipmentManager;
    [HideInInspector] public PlayerCombatManager PlayerCombatManager;
    [HideInInspector] public PlayerInteractionManager PlayerInteractionManager;



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
        PlayerInteractionManager = GetComponent<PlayerInteractionManager>();
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

        // Show Hp Bar If Not Own Character
        if (!IsOwner && CharacterUIManager.HasFloatingUIBar) {
            CharacterNetworkManager.CurrentHealth.OnValueChanged += CharacterUIManager.OnHPChanged;
        }

        // Stats
        PlayerNetworkManager.CurrentHealth.OnValueChanged += PlayerNetworkManager.CheckHP;

        // Lock On
        PlayerNetworkManager.IsLockedOn.OnValueChanged += PlayerNetworkManager.OnIsLockedOnChanged;
        PlayerNetworkManager.CurrentTargetNetworkObjectID.OnValueChanged += PlayerNetworkManager.OnLockOnTargetIDChange;

        // Equipment
        PlayerNetworkManager.CurrentRightHandWeaponID.OnValueChanged += PlayerNetworkManager.OnCurrentRightHandWeaponIDChange;
        PlayerNetworkManager.CurrentLeftHandWeaponID.OnValueChanged += PlayerNetworkManager.OnCurrentLeftHandWeaponIDChange;
        PlayerNetworkManager.CurrentWeaponBeingUsed.OnValueChanged += PlayerNetworkManager.OnCurrentWeaponBeingUsedIDChange;
        PlayerNetworkManager.IsBlocking.OnValueChanged += PlayerNetworkManager.OnIsBlockingChanged;

        // Flags
        PlayerNetworkManager.IsChargingAttack.OnValueChanged += PlayerNetworkManager.OnIsChargingAttackChanged;

        // Load Stats When Joining
        if (IsOwner && !IsServer) {
            LoadDataFromCurrentCharacterData(ref WorldSaveGameManager.Instance.CurrentCharacterData);
        }
    }

    public override void OnNetworkDespawn() {
        base.OnNetworkDespawn();

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;

        if (IsOwner) {
            // Update Total Amount Of (Health/Stamina) With Level (Vitality/Endurance)
            PlayerNetworkManager.Vitality.OnValueChanged -= PlayerNetworkManager.SetNewMaxHealthValue;
            PlayerNetworkManager.Endurance.OnValueChanged -= PlayerNetworkManager.SetNewMaxStaminaValue;

            // Updates HUD UI When Value Change (HP/Stamina)
            PlayerNetworkManager.CurrentHealth.OnValueChanged -= PlayerUIManager.Instance.PlayerUIHudManager.SetNewHealthValue;
            PlayerNetworkManager.CurrentStamina.OnValueChanged -= PlayerUIManager.Instance.PlayerUIHudManager.SetNewStaminaValue;
            PlayerNetworkManager.CurrentStamina.OnValueChanged -= PlayerStatsManager.ResetStaminaRegenTimer;

        }
        if (!IsOwner && CharacterUIManager.HasFloatingUIBar) {
            CharacterNetworkManager.CurrentHealth.OnValueChanged -= CharacterUIManager.OnHPChanged;
        }

        // Stats
        PlayerNetworkManager.CurrentHealth.OnValueChanged -= PlayerNetworkManager.CheckHP;

        // Lock On
        PlayerNetworkManager.IsLockedOn.OnValueChanged -= PlayerNetworkManager.OnIsLockedOnChanged;
        PlayerNetworkManager.CurrentTargetNetworkObjectID.OnValueChanged -= PlayerNetworkManager.OnLockOnTargetIDChange;

        // Equipment
        PlayerNetworkManager.CurrentRightHandWeaponID.OnValueChanged -= PlayerNetworkManager.OnCurrentRightHandWeaponIDChange;
        PlayerNetworkManager.CurrentLeftHandWeaponID.OnValueChanged -= PlayerNetworkManager.OnCurrentLeftHandWeaponIDChange;
        PlayerNetworkManager.CurrentWeaponBeingUsed.OnValueChanged -= PlayerNetworkManager.OnCurrentWeaponBeingUsedIDChange;
        PlayerNetworkManager.IsBlocking.OnValueChanged -= PlayerNetworkManager.OnIsBlockingChanged;

        // Flags
        PlayerNetworkManager.IsChargingAttack.OnValueChanged -= PlayerNetworkManager.OnIsChargingAttackChanged;
    }

    protected override void OnEnable() {
        base.OnEnable();


    }

    protected override void OnDisable() {
        base.OnDisable();

    }

    void OnClientConnectedCallback(ulong clientID) {
        // Keep List Of Active Players
        WorldGameSessionManager.Instance.AddPlayerToActivePlayersList(this);

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
            IsDead.Value = false;
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

        // Sync Block Status
        PlayerNetworkManager.OnIsBlockingChanged(false, PlayerNetworkManager.IsBlocking.Value);

        // Lock On
        if (PlayerNetworkManager.IsLockedOn.Value) {
            PlayerNetworkManager.OnLockOnTargetIDChange(0, PlayerNetworkManager.CurrentTargetNetworkObjectID.Value);
        }
    }

}