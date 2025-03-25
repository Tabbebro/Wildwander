using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Light Attack Action")]
public class LightAttackWeaponItemAction : WeaponItemAction
{
    [SerializeField] string lightAttack01 = "Main_Light_Attack_01"; // Main Hand Attack Animation

    public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction) {
        base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

        // TODO: Check For Stops
        if(!playerPerformingAction.IsOwner) { return; }

        if (playerPerformingAction.PlayerNetworkManager.CurrentStamina.Value <= 0) { return; }

        if (!playerPerformingAction.IsGrounded) { return; }

        PerformLightAttack(playerPerformingAction, weaponPerformingAction);
    }

    private void PerformLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction) {
        // Right Hand
        if (playerPerformingAction.PlayerNetworkManager.IsUsingRightHand.Value) {
            playerPerformingAction.PlayerAnimatorManager.PlayTargetAttackActionAnimation(lightAttack01, true);
        }
        // Left Hand
        if (playerPerformingAction.PlayerNetworkManager.IsUsingLeftHand.Value) {

        }
    }
}
