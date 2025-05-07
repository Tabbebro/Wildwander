using UnityEngine;

[CreateAssetMenu(menuName = "AI/States/Boss Sleep")]
public class SleepState : AIState
{
    public override AIState Tick(AICharacterManager aiCharacter) {
        return base.Tick(aiCharacter);
    }
}
