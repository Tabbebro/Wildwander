using UnityEngine;

public class AiCharacterAnimatorManager : CharacterAnimatorManager
{
    AICharacterManager _aiCharacter;

    protected override void Awake() {
        base.Awake();
        _aiCharacter = GetComponent<AICharacterManager>();
    }
    private void OnAnimatorMove() {
        // Host
        if (_aiCharacter.IsOwner) {
            if (!_aiCharacter.IsGrounded) { return; }

            Vector3 velocity = _aiCharacter.Animator.deltaPosition;

            _aiCharacter.CharacterController.Move(velocity);
            _aiCharacter.transform.rotation *= _aiCharacter.Animator.deltaRotation;
        }
        // Client
        else {
            if (!_aiCharacter.IsGrounded) { return; }

            Vector3 velocity = _aiCharacter.Animator.deltaPosition;

            _aiCharacter.CharacterController.Move(velocity);
            _aiCharacter.transform.position = Vector3.SmoothDamp(transform.position, _aiCharacter.CharacterNetworkManager.NetworkPosition.Value, 
                ref _aiCharacter.CharacterNetworkManager.NetworkPositionVelocity, _aiCharacter.CharacterNetworkManager.NetworkPositionSmoothTime);
            _aiCharacter.transform.rotation *= _aiCharacter.Animator.deltaRotation;
        }
    }
}
