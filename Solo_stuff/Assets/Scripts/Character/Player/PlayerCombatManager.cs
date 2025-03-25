using UnityEngine;

public class PlayerCombatManager : CharacterCombatManager
{
    PlayerManager _player;

    public WeaponItem CurrentWeaponBeingUsed;

    protected override void Awake() {
        base.Awake();

        _player = GetComponent<PlayerManager>();
    }

    public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction) {
        
        // Perform Action
        weaponAction.AttemptToPerformAction(_player, weaponPerformingAction);

        // Notify The Server That Action Is Performed

    }
}
