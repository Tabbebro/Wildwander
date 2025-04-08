using UnityEngine;
using Unity.Netcode;

public class PlayerCombatManager : CharacterCombatManager
{
    PlayerManager _player;

    public WeaponItem CurrentWeaponBeingUsed;

    

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
}
