using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Light Attack Action")]
public class LightAttackWeaponItemAction : WeaponItemAction
{
    [Header("Light Attack")]
    [SerializeField] string _lightAttack01 = "Main_Light_Attack_01"; // Main Hand Attack Animation
    [SerializeField] string _lightAttack02 = "Main_Light_Attack_02"; // Main Hand Attack Combo Animation
    [SerializeField] string _lightAttack03 = "Main_Light_Attack_03"; // Main Hand Attack Combo Animation

    [Header("Running Attacks")]
    [SerializeField] string _runAttack01 = "Main_Run_Attack_01"; // Main Hand Run Attack Animation

    [Header("Rolling Attacks")]
    [SerializeField] string _rollAttack01 = "Main_Roll_Attack_01"; // Main Hand Roll Attack Animation

    [Header("Backstep Attacks")]
    [SerializeField] string _backstepAttack01 = "Main_Backstep_Attack_01"; // Main Hand Backstep Attack Animation

    public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction) {
        base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

        // TODO: Check For Stops
        if(!playerPerformingAction.IsOwner) { return; }

        if (playerPerformingAction.PlayerNetworkManager.CurrentStamina.Value <= 0) { return; }

        if (!playerPerformingAction.CharacterMovementManager.IsGrounded) { return; }

        if (playerPerformingAction.IsOwner) {
            playerPerformingAction.PlayerNetworkManager.IsAttacking.Value = true;
        }

        // If Sprinting Do A Sprint Attack
        if (playerPerformingAction.CharacterNetworkManager.IsSprinting.Value) {
            PerformRunningAttack(playerPerformingAction, weaponPerformingAction);
            return;
        }

        // If Rolling Do A Roll Attack
        if (playerPerformingAction.CharacterCombatManager.CanPerformRollingAttack) {
            PerformRollingAttack(playerPerformingAction, weaponPerformingAction);
            return;
        }

        // If Rolling Do A Roll Attack
        if (playerPerformingAction.CharacterCombatManager.CanPerformBackstepAttack) {
            PerformBackstepAttack(playerPerformingAction, weaponPerformingAction);
            return;
        }

        PerformLightAttack(playerPerformingAction, weaponPerformingAction);
    }

    void PerformLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction) {
        // If Already Attacking & Can Combo Do Combo
        if (playerPerformingAction.PlayerCombatManager.CanComboWithMainHandWeapon && playerPerformingAction.IsPerformingAction) {
            playerPerformingAction.PlayerCombatManager.CanComboWithMainHandWeapon = false;

            // Perform Attack Based On Previous Attack
            if (playerPerformingAction.CharacterCombatManager.LastAttackAnimationPerformed == _lightAttack01 && _lightAttack02 != "") {
                playerPerformingAction.PlayerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack02, _lightAttack02, true);
            }
            else if (playerPerformingAction.CharacterCombatManager.LastAttackAnimationPerformed == _lightAttack02 && _lightAttack03 != "") {
                playerPerformingAction.PlayerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack03, _lightAttack03, true);
            }
            else {
                playerPerformingAction.PlayerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack01, _lightAttack01, true);
            }
        }

        // Else If Not Attacking Do Regular Attack
        else if (!playerPerformingAction.IsPerformingAction) {
            playerPerformingAction.PlayerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack01, _lightAttack01, true);
        }

    }

    void PerformRunningAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction) {

        // TODO: Check For Two Handing

        playerPerformingAction.PlayerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.RunningAttack01, _runAttack01, true);
    }

    void PerformRollingAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction) {

        // TODO: Check For Two Handing
        playerPerformingAction.PlayerCombatManager.CanPerformRollingAttack = false;
        playerPerformingAction.PlayerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.RollingAttack01, _rollAttack01, true);
    }

    void PerformBackstepAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction) {

        // TODO: Check For Two Handing
        playerPerformingAction.PlayerCombatManager.CanPerformBackstepAttack = false;
        playerPerformingAction.PlayerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.BackstepAttack01, _backstepAttack01, true);
    }
}
