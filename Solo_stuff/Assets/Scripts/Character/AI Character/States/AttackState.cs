using System.Transactions;
using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/States/Attack")]
public class AttackState : AIState
{
    [Header("Current Attack")]
    [HideInInspector] public AICharacterAttackAction CurrentAttack;
    [HideInInspector] public bool WillPerformCombo;

    [Header("State Flags")]
    protected bool _hasPerformedAttack = false;
    protected bool _hasPerformedCombo = false;

    [Header("Pivot After Attack")]
    [SerializeField] protected bool _pivotAfterAttack = false;


    public override AIState Tick(AICharacterManager aiCharacter) {
        if (aiCharacter.AICharacterCombatManager.CurrentTarget == null) {
            return SwitchState(aiCharacter, aiCharacter.Idle);
        }

        if (aiCharacter.AICharacterCombatManager.CurrentTarget.IsDead.Value) {
            return SwitchState(aiCharacter, aiCharacter.Idle);
        }

        aiCharacter.CharacterAnimatorManager.UpdateAnimatorMovementParameters(0, 0, false);

        // Perform Combo
        if (WillPerformCombo && !_hasPerformedCombo) {
            if (CurrentAttack.ComboAction != null) {
                //_hasPerformedCombo = true;
                //CurrentAttack.ComboAction.AttemptToPerformAction(aiCharacter);
            }
        }

        if (!_hasPerformedAttack) {
            if (aiCharacter.AICharacterCombatManager.ActionRecoveryTimer > 0) {
                return this;
            }

            if (aiCharacter.IsPerformingAction) {
                return this;
            }

            PerformAttack(aiCharacter);

            return this;
        }

        if (_pivotAfterAttack) {
            aiCharacter.AICharacterCombatManager.PivotTowardsTarget(aiCharacter);
        }

        return SwitchState(aiCharacter, aiCharacter.CombatStance);
    }

    protected void PerformAttack(AICharacterManager aiCharacter) {
        _hasPerformedAttack = true;
        CurrentAttack.AttemptToPerformAction(aiCharacter);
        aiCharacter.AICharacterCombatManager.ActionRecoveryTimer = CurrentAttack.ActionRecoveryTime;
    }

    protected override void ResetStateFlags(AICharacterManager aiCharacter) {
        base.ResetStateFlags(aiCharacter);

        _hasPerformedAttack = false;
        _hasPerformedCombo = false;
    }
}
