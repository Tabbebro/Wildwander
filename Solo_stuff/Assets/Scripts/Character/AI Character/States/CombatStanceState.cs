using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/States/Combat Stance")]
public class CombatStanceState : AIState
{

    [Header("Attacks")]
    public List<AICharacterAttackAction> AICharacterAttacks = new();
    protected List<AICharacterAttackAction> _potentialAttacks;
    AICharacterAttackAction _chosenAttack;
    AICharacterAttackAction _previousAttack;
    protected bool _hasAttack = false;

    [Header("Combo")]
    [SerializeField] protected bool _canPerformCombo = false;
    [SerializeField] protected int _chanceToPerformCombo = 25;
    protected bool _hasRolledForComboChance = false;

    [Header("Engagement Distance")]
    [SerializeField] protected float _maxEngagementDistance = 5;

    public override AIState Tick(AICharacterManager aiCharacter) {
        if (aiCharacter.IsPerformingAction) { return this; }
        if (!aiCharacter.NavmeshAgent.enabled) { aiCharacter.NavmeshAgent.enabled = true; }

        // Rotate Towards Target If Outside FOV
        if (!aiCharacter.AICharacterNetworkManager.IsMoving.Value) {
            if (aiCharacter.AICharacterCombatManager.ViewableAngle < -30 || aiCharacter.AICharacterCombatManager.ViewableAngle > 30) {
                aiCharacter.AICharacterCombatManager.PivotTowardsTarget(aiCharacter);
            }
        }

        // If No Target Change State To Idle
        if (aiCharacter.AICharacterCombatManager.CurrentTarget == null) {
            return SwitchState(aiCharacter, aiCharacter.Idle);
        }

        if (!_hasAttack) {
            GetNewAttack(aiCharacter);
        }
        else {
            // Go To Attack State
        }

        // If Outside Max Engagement Distance Change State To Pursue Target
        if (aiCharacter.AICharacterCombatManager.DistanceFromTarget > _maxEngagementDistance) {
            return SwitchState(aiCharacter, aiCharacter.PursueTarget);
        }

        NavMeshPath path = new();
        aiCharacter.NavmeshAgent.CalculatePath(aiCharacter.AICharacterCombatManager.CurrentTarget.transform.position, path);
        aiCharacter.NavmeshAgent.SetPath(path);

        return this;
    }

    protected virtual void GetNewAttack(AICharacterManager aiCharacter) {
        _potentialAttacks = new();

        foreach (AICharacterAttackAction potentialAttack in _potentialAttacks) {
            // Check If Target Is Too Close
            if (potentialAttack.MinAttackDistance > aiCharacter.AICharacterCombatManager.DistanceFromTarget) { continue; }
            // Check If Target Is Too Far Away
            if (potentialAttack.MaxAttackDistance < aiCharacter.AICharacterCombatManager.DistanceFromTarget) { continue; }

            // Check If Target Is Outside Min FOV
            if (potentialAttack.MinAttackAngle < aiCharacter.AICharacterCombatManager.ViewableAngle) { continue; }
            // Check If Target Is Outside Max FOV
            if (potentialAttack.MaxAttackAngle > aiCharacter.AICharacterCombatManager.ViewableAngle) { continue; }

            _potentialAttacks.Add(potentialAttack);
        }

        if (_potentialAttacks.Count <= 0) { return; }

        var totalWeight = 0;

        foreach (AICharacterAttackAction attack in _potentialAttacks) {
            totalWeight += attack.AttackWeight;
        }

        var randomWeightValue = Random.Range(1, totalWeight + 1);
        var processedWeight = 0;

        foreach (AICharacterAttackAction attack in _potentialAttacks) {
            processedWeight += attack.AttackWeight;

            if (randomWeightValue <= processedWeight) {
                _chosenAttack = attack;
                _previousAttack = _chosenAttack;
                _hasAttack = true;
            }
        }

    }

    protected virtual bool RollForOutcomeChance(int outcomeChance) {
        bool outcomeWillBePerformed = false;

        int randomPercentage = Random.Range(0, 101);

        if (randomPercentage < outcomeChance) {
            outcomeWillBePerformed = true;
        }

        return outcomeWillBePerformed;
    }

    protected override void ResetStateFlags(AICharacterManager aiCharacter) {
        base.ResetStateFlags(aiCharacter);

        _hasAttack = false;
        _hasRolledForComboChance = false;
    }
}
