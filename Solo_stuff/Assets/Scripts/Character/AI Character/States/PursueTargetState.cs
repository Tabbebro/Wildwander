using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/States/Pursue Target")]
public class PursueTargetState : AIState
{
    public override AIState Tick(AICharacterManager aiCharacter) {

        // Check For Performing Action
        if(aiCharacter.IsPerformingAction) { return this; }

        // Check If Target Is Null
        if(aiCharacter.AICharacterCombatManager.CurrentTarget == null) { return SwitchState(aiCharacter, aiCharacter.Idle); }

        // Check If Navmesh Agent Is Active
        if(!aiCharacter.NavmeshAgent.enabled) {
            aiCharacter.NavmeshAgent.enabled = true;
        }

        // If Target Outside FOV Pivot To Face Target
        if (aiCharacter.AICharacterCombatManager.ViewableAngle < aiCharacter.AICharacterCombatManager.MinFOV ||
            aiCharacter.AICharacterCombatManager.ViewableAngle > aiCharacter.AICharacterCombatManager.MaxFOV) {
            aiCharacter.AICharacterCombatManager.PivotTowardsTarget(aiCharacter);
        }

        aiCharacter.AICharacterMovementManager.RotateTowardsAgent(aiCharacter);

        // If Near Change To Combat State
        if (aiCharacter.AICharacterCombatManager.DistanceFromTarget <= aiCharacter.NavmeshAgent.stoppingDistance) {
            return SwitchState(aiCharacter, aiCharacter.CombatStance);
        }

        // TODO: If Target Too Far Lose Agro

        // Pursue Target
        NavMeshPath path = new();
        aiCharacter.NavmeshAgent.CalculatePath(aiCharacter.AICharacterCombatManager.CurrentTarget.transform.position, path);
        aiCharacter.NavmeshAgent.SetPath(path);


        return this;
    }
}
