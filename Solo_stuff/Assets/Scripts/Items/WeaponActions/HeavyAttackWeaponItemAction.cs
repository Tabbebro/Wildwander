using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Heavy Attack Action")]
public class HeavyAttackWeaponItemAction : WeaponItemAction 
{
    [SerializeField] string heavyAttack01 = "Main_Heavy_Attack_01"; // Main Hand Attack Animation
    [SerializeField] string heavyAttack02 = "Main_Heavy_Attack_02"; // Main Hand Attack Animation
    [SerializeField] string heavyAttack03 = "Main_Heavy_Attack_03"; // Main Hand Attack Animation

    public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction) {
        base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

        // TODO: Check For Stops
        if (!playerPerformingAction.IsOwner) { return; }

        if (playerPerformingAction.PlayerNetworkManager.CurrentStamina.Value <= 0) { return; }

        if (!playerPerformingAction.CharacterMovementManager.IsGrounded) { return; }

        if (playerPerformingAction.IsOwner) {
            playerPerformingAction.PlayerNetworkManager.IsAttacking.Value = true;
        }

        PerformHeavyAttack(playerPerformingAction, weaponPerformingAction);
    }

    private void PerformHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction) {
        // If Already Attacking & Can Combo Do Combo
        if (playerPerformingAction.PlayerCombatManager.CanComboWithMainHandWeapon && playerPerformingAction.IsPerformingAction) {
            playerPerformingAction.PlayerCombatManager.CanComboWithMainHandWeapon = false;

            // Perform Attack Based On Previous Attack
            if (playerPerformingAction.CharacterCombatManager.LastAttackAnimationPerformed == heavyAttack01 && heavyAttack02 != "") {
                playerPerformingAction.PlayerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack02, heavyAttack02, true);
            }
            else if (playerPerformingAction.CharacterCombatManager.LastAttackAnimationPerformed == heavyAttack02 && heavyAttack01 != "") {
                playerPerformingAction.PlayerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack03, heavyAttack03, true);
            }
            else {
                playerPerformingAction.PlayerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack01, heavyAttack01, true);
            }
        }
        // Else If Not Attacking Do Regular Attack
        else if (!playerPerformingAction.IsPerformingAction) {
            playerPerformingAction.PlayerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.HeavyAttack01, heavyAttack01, true);
        }
    }
}
