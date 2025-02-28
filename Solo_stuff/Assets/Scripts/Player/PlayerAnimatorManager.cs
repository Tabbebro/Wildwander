using UnityEngine;

public class PlayerAnimatorManager : CharacterAnimatorManager
{
    PlayerManager _player;

    protected override void Awake() {
        base.Awake();

        _player = GetComponent<PlayerManager>();
    }
    private void OnAnimatorMove() {
        if (_player.applyRootMotion) {
            Vector3 velocity = _player._animator.deltaPosition;
            if (_player._animator.GetCurrentAnimatorStateInfo(1).IsName("Roll_Forward_01")) {
                velocity *= 1.25f;
            }
            _player._characterController.Move(velocity);
            _player.transform.rotation *= _player._animator.deltaRotation;
        }
    }
}
