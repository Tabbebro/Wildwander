using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Heavy Attack Action")]
public class HeavyAttackWeaponItemAction : WeaponItemAction 
{
    [SerializeField] string heavyAttack01 = "Main_Heavy_Attack_01"; // Main Hand Attack Animation

    public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction) {
        base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

        // TODO: Check For Stops
        if (!playerPerformingAction.IsOwner) { return; }

        if (playerPerformingAction.PlayerNetworkManager.CurrentStamina.Value <= 0) { return; }

        if (!playerPerformingAction.IsGrounded) { return; }

        PerformHeavyAttack(playerPerformingAction, weaponPerformingAction);
    }

    private void PerformHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction) {
        // Right Hand
        if (playerPerformingAction.PlayerNetworkManager.IsUsingRightHand.Value) {
            playerPerformingAction.PlayerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.HeavyAttack01, heavyAttack01, true);
        }
        // Left Hand
        if (playerPerformingAction.PlayerNetworkManager.IsUsingLeftHand.Value) {

        }
    }
}
