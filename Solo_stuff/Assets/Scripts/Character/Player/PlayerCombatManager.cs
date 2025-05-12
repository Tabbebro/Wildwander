using UnityEngine;
using Unity.Netcode;

public class PlayerCombatManager : CharacterCombatManager
{
    PlayerManager _player;

    public WeaponItem CurrentWeaponBeingUsed;

    [Header("Flags")]
    public bool CanComboWithMainHandWeapon = false;
    //public bool CanComboWithOffHandWeapon = false;

    protected override void Awake() {
        base.Awake();

        _player = GetComponent<PlayerManager>();
    }

    public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction) {
        
        if (!_player.IsOwner) { return; }
        // Perform Action
        weaponAction.AttemptToPerformAction(_player, weaponPerformingAction);

        // Notify The Server That Action Is Performed
        _player.PlayerNetworkManager.NotifyServerOfWeaponActionServerRpc(NetworkManager.Singleton.LocalClientId, weaponAction.actionID, weaponPerformingAction.ItemID);
    }

    public virtual void DrainStaminaBasedOnAttack() {
        if (!_player.IsOwner) { return; }

        if (CurrentWeaponBeingUsed == null) { return; }

        float staminaDrainded = 0;

        // TODO: ADD More When There Are More Attack Types
        switch (CurrentAttackType) {
            case AttackType.LightAttack01:
                staminaDrainded = CurrentWeaponBeingUsed.BaseStaminaCost * CurrentWeaponBeingUsed.LigthAttackStaminaModifier;
                break;
            case AttackType.LightAttack02:
                staminaDrainded = CurrentWeaponBeingUsed.BaseStaminaCost * CurrentWeaponBeingUsed.LigthAttackStaminaModifier;
                break;
            case AttackType.HeavyAttack01:
                staminaDrainded = CurrentWeaponBeingUsed.BaseStaminaCost * CurrentWeaponBeingUsed.HeavyAttackStaminaModifier;
                break;
            case AttackType.HeavyAttack02:
                staminaDrainded = CurrentWeaponBeingUsed.BaseStaminaCost * CurrentWeaponBeingUsed.HeavyAttackStaminaModifier;
                break;
            case AttackType.HeavyAttackHold01:
                staminaDrainded = CurrentWeaponBeingUsed.BaseStaminaCost * CurrentWeaponBeingUsed.HeavyAttackHoldStaminaModifier;
                break;
            case AttackType.HeavyAttackHold02:
                staminaDrainded = CurrentWeaponBeingUsed.BaseStaminaCost * CurrentWeaponBeingUsed.HeavyAttackHoldStaminaModifier;
                break;
            case AttackType.RunningAttack01:
                staminaDrainded = CurrentWeaponBeingUsed.BaseStaminaCost * CurrentWeaponBeingUsed.RunAttackStaminaModifier;
                break;
            case AttackType.RollingAttack01:
                staminaDrainded = CurrentWeaponBeingUsed.BaseStaminaCost * CurrentWeaponBeingUsed.RollAttackStaminaModifier;
                break;
            case AttackType.BackstepAttack01:
                staminaDrainded = CurrentWeaponBeingUsed.BaseStaminaCost * CurrentWeaponBeingUsed.BackstepAttackStaminaModifier;
                break;
            default:
                break;
        }

        _player.PlayerNetworkManager.CurrentStamina.Value -= Mathf.RoundToInt(staminaDrainded);
    }

    public override void SetTarget(CharacterManager newTarget) {
        base.SetTarget(newTarget);

        if (_player.IsOwner) {
            PlayerCamera.Instance.SetLockCameraHeight();
        }
    }

    // Animation Event Calls
    public override void EnableCanDoCombo() {
        if (_player.PlayerNetworkManager.IsUsingRightHand.Value) {
            _player.PlayerCombatManager.CanComboWithMainHandWeapon = true;
        }
        else {
            // TODO: Enable Combo For Offhand
        }
    }

    public override void DisableCanDoCombo() {
        _player.PlayerCombatManager.CanComboWithMainHandWeapon = false;
        // TODO: Disable Combo For Offhand
    }
}

