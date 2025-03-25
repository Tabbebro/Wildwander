using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

public class PlayerNetworkManager : CharacterNetworkManager
{
    PlayerManager _player;

    public NetworkVariable<FixedString64Bytes> CharacterName = new NetworkVariable<FixedString64Bytes>("Character", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Equipment")]
    public NetworkVariable<int> CurrentWeaponBeingUsed = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> CurrentRightHandWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> CurrentLeftHandWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> IsUsingRightHand = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> IsUsingLeftHand = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    protected override void Awake() {
        base.Awake();
        _player = GetComponent<PlayerManager>();
    }

    public void SetCharacterActionHand(bool rightHandedAction) {
        // Using Right Hand
        if (rightHandedAction) { 
            IsUsingRightHand.Value = true;
            IsUsingLeftHand.Value = false;
        }
        // Using Left Hand
        else {
            IsUsingLeftHand.Value = true;
            IsUsingRightHand.Value = false;
        }
    }

    public void SetNewMaxHealthValue(int oldVitality, int newVitality) {
        print("Vitality Changed from: " + oldVitality + " To: " + newVitality);
        MaxHealth.Value = _player.PlayerStatsManager.CalculateHealthBasedOnLevel(newVitality);
        PlayerUIManager.Instance.PlayerUIHudManager.SetMaxHealthValue(MaxHealth.Value);
        CurrentHealth.Value = MaxHealth.Value;
    }

    public void SetNewMaxStaminaValue(int oldEndurance, int newEndurance) {
        MaxStamina.Value = _player.PlayerStatsManager.CalculateStaminaBasedOnLevel(newEndurance);
        PlayerUIManager.Instance.PlayerUIHudManager.SetMaxStaminaValue(MaxStamina.Value);
        CurrentStamina.Value = MaxStamina.Value;
    }

    public void OnCurrentRightHandWeaponIDChange(int oldID, int newID) {
        WeaponItem newWeapon = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newID));
        _player.PlayerInventoryManager.CurrentRightHandWeapon = newWeapon;
        _player.PlayerEquipmentManager.LoadRightWeapon();
    }

    public void OnCurrentLeftHandWeaponIDChange(int oldID, int newID) {
        WeaponItem newWeapon = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newID));
        _player.PlayerInventoryManager.CurrentLeftHandWeapon = newWeapon;
        _player.PlayerEquipmentManager.LoadLeftWeapon();
    }

    public void OnCurrentWeaponBeingUsedIDChange(int oldID, int newID) {
        WeaponItem newWeapon = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newID));
        _player.PlayerCombatManager.CurrentWeaponBeingUsed = newWeapon;
        _player.PlayerEquipmentManager.LoadLeftWeapon();
    }
}
