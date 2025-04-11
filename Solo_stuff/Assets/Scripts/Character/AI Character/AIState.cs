using UnityEngine;

public class AIState : ScriptableObject
{
    public virtual AIState Tick(AICharacterManager aiCharacter) {
        return this;
    }
}
