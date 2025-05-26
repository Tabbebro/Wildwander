using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Off Hand Melee Action")]
public class OffHandMeleeAction : WeaponItemAction
{
    public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction) {
        base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

        // TODO: Check For PowerStance

        if (!playerPerformingAction.PlayerCombatManager.CanPerformBlock) { return; }

        // Check For Attacking Flag
        if (playerPerformingAction.PlayerNetworkManager.IsAttacking.Value) {
            if (playerPerformingAction.IsOwner) {
                playerPerformingAction.PlayerNetworkManager.IsBlocking.Value = false;
            }

            return;
        }

        if (playerPerformingAction.PlayerNetworkManager.IsBlocking.Value) { return; }

        if (playerPerformingAction.IsOwner) {
            playerPerformingAction.PlayerNetworkManager.IsBlocking.Value = true;
        }
    }
}
