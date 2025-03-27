using UnityEngine;
using Unity.Netcode;

public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager _character;

    int _horizontal;
    int _vertical;

    protected virtual void Awake() {
        _character = GetComponent<CharacterManager>();

        _vertical = Animator.StringToHash("Vertical");
        _horizontal = Animator.StringToHash("Horizontal");
    }

    public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue, bool isSprinting) {

        float horizontal = horizontalValue;
        float vertical = verticalValue;

        if (isSprinting) {
            vertical = 2;
        }

        _character.Animator.SetFloat(_horizontal, horizontal, 0.1f, Time.deltaTime);
        _character.Animator.SetFloat(_vertical, vertical, 0.1f, Time.deltaTime);

    }

    public virtual void PlayTargetActionAnimation(
        string targetAnimation, 
        bool isPerformingAnimation, 
        bool applyRootMotion = true, 
        bool canRotate = false, 
        bool canMove = false
        ) {

        _character.ApplyRootMotion = applyRootMotion;
        _character.Animator.CrossFade(targetAnimation, 0.2f);

        _character.IsPerformingAction = isPerformingAnimation;
        _character.CanRotate = canRotate;
        _character.CanMove = canMove;

        _character.CharacterNetworkManager.NotifyServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
    }

    public virtual void PlayTargetAttackActionAnimation(
        AttackType attackType,
        string targetAnimation,
        bool isPerformingAnimation,
        bool applyRootMotion = true,
        bool canRotate = false,
        bool canMove = false
    ) {

        // TODO: Combo Check
        // TODO: Keep Track Of Attack Type
        // TODO: Update Animations
        // TODO: Parry?

        _character.CharacterCombatManager.CurrentAttackType = attackType;

        _character.ApplyRootMotion = applyRootMotion;
        _character.Animator.CrossFade(targetAnimation, 0.2f);

        _character.IsPerformingAction = isPerformingAnimation;
        _character.CanRotate = canRotate;
        _character.CanMove = canMove;

        _character.CharacterNetworkManager.NotifyServerOfAttackActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
    }
}
