using UnityEngine;

public class AICharacterMovementManager : CharacterMovementManager
{
    public void RotateTowardsAgent(AICharacterManager aiCharacter) {
        if (aiCharacter.AICharacterNetworkManager.IsMoving.Value) {
            aiCharacter.transform.rotation = aiCharacter.NavmeshAgent.transform.rotation;
        }
    }
}
