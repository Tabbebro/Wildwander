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
}
