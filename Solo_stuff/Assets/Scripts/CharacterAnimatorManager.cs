using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager _character;

    protected virtual void Awake() {
        _character = GetComponent<CharacterManager>();
    }

    public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue) {
        _character._animator.SetFloat("Horizontal", horizontalValue, 0.1f, Time.deltaTime);
        _character._animator.SetFloat("Vertical", verticalValue, 0.1f, Time.deltaTime);
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
