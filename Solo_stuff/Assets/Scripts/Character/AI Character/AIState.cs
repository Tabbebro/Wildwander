using UnityEngine;

public class AIState : ScriptableObject
{
    public virtual AIState Tick(AICharacterManager aiCharacter) {
        return this;
    }

    protected virtual AIState SwitchState(AICharacterManager aiCharacter, AIState newState) {
        ResetStateFlags(aiCharacter);
        return newState;
    }

    protected virtual void ResetStateFlags(AICharacterManager aiCharacter) {
        // Reset State Flags Here
    }
}
