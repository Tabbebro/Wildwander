using UnityEngine;

[CreateAssetMenu(menuName = "AI/States/Idle")]
public class IdleState : AIState
{
    public override AIState Tick(AICharacterManager aiCharacter) {
        
        if (aiCharacter.CharacterCombatManager.CurrentTarget != null) {
            // TODO: Return The Pursue State
            Debug.Log("AI Has Target");
            return this;
        }
        else {
            // Continue To Search For Target
            aiCharacter.AICharacterCombatManager.FindTargetLineOfSight(aiCharacter);
            Debug.Log("Searching For Target");
            return this;
        }

    }
}
