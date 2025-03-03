using UnityEngine;

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

        _character._animator.SetFloat(_horizontal, horizontal, 0.1f, Time.deltaTime);
        _character._animator.SetFloat(_vertical, vertical, 0.1f, Time.deltaTime);

    }

    public virtual void PlayTargetActionAnimation(
        string targetAnimation, 
        bool isPerformingAnimation, 
        bool applyRootMotion = true, 
        bool canRotate = false, 
        bool canMove = false
        ) {

        _character.applyRootMotion = applyRootMotion;
        _character._animator.CrossFade(targetAnimation, 0.2f);

        _character.isPerformingAction = isPerformingAnimation;
        _character.canRotate = canRotate;
        _character.canMove = canMove;
    }
}
