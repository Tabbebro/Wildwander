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
            _player._characterController.Move(velocity);
            _player.transform.rotation *= _player._animator.deltaRotation;
        }
    }
}
