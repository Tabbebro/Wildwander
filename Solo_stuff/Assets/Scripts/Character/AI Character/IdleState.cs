using UnityEngine;

[CreateAssetMenu(menuName = "AI/States/Idle")]
public class IdleState : AIState
{
    public override AIState Tick(AICharacterManager aiCharacter) {
        
        if (aiCharacter.CharacterCombatManager.CurrentTarget != null) {
            return SwitchState(aiCharacter, aiCharacter.PursueTarget);
        }
        else {
            // Continue To Search For Target
            aiCharacter.AICharacterCombatManager.FindTargetLineOfSight(aiCharacter);
            return this;
        }

    }
}
