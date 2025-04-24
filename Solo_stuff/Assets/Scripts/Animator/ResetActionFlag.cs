using UnityEngine;

public class ResetActionFlag : StateMachineBehaviour
{
    CharacterManager _character;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_character == null) {
            _character = animator.GetComponent<CharacterManager>();
        }

        // Reset Flags
        _character.IsPerformingAction = false;
        _character.CharacterMovementManager.CanMove = true;
        _character.CharacterMovementManager.CanRotate = true;
        _character.CharacterAnimatorManager.ApplyRootMotion = false;
        _character.CharacterMovementManager.isRolling = false;
        _character.CharacterAnimatorManager.DisableCanDoCombo();

        if (_character.IsOwner) {
            _character.CharacterNetworkManager.IsJumping.Value = false;
            _character.CharacterNetworkManager.IsInvulnerable.Value = false;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
