using UnityEngine;

public class PlayerAnimatorManager : CharacterAnimatorManager
{
    PlayerManager _player;

    protected override void Awake() {
        base.Awake();

        _player = GetComponent<PlayerManager>();
    }
    private void OnAnimatorMove() {
        if (_player.ApplyRootMotion) {
            Vector3 velocity = _player.Animator.deltaPosition;
            if (_player.Animator.GetCurrentAnimatorStateInfo(1).IsName("Roll_Forward_01")) {
                velocity *= 1.25f;
            }
            _player.CharacterController.Move(velocity);
            _player.transform.rotation *= _player.Animator.deltaRotation;
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
