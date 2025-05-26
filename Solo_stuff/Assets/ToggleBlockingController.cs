using UnityEngine;

public class ToggleBlockingController : StateMachineBehaviour
{

    PlayerManager _player;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_player == null) {
            _player = animator.GetComponent<PlayerManager>();
        }

        if (_player == null) { return; }

        // TODO: Check For 2h Status

        if (_player.PlayerNetworkManager.IsBlocking.Value) {
            _player.PlayerAnimatorManager.UpdateAnimatorController(_player.PlayerInventoryManager.CurrentLeftHandWeapon.WeaponAnimator);
        }
        else{
            _player.PlayerAnimatorManager.UpdateAnimatorController(_player.PlayerInventoryManager.CurrentRightHandWeapon.WeaponAnimator);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_player == null) {
            _player = animator.GetComponent<PlayerManager>();
        }

        if (_player == null) { return; }

        // TODO: Check For 2h Status

        if (_player.PlayerNetworkManager.IsBlocking.Value) {
            _player.PlayerAnimatorManager.UpdateAnimatorController(_player.PlayerInventoryManager.CurrentLeftHandWeapon.WeaponAnimator);
        }
        else {
            _player.PlayerAnimatorManager.UpdateAnimatorController(_player.PlayerInventoryManager.CurrentRightHandWeapon.WeaponAnimator);
        }
    }

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
