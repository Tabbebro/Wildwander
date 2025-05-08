using UnityEngine;

public class AIBossCharacterNetworkManager : AICharacterNetworkManager
{
    AIBossCharacterManager _aiBossCharacter;

    protected override void Awake() {
        base.Awake();
        _aiBossCharacter = GetComponent<AIBossCharacterManager>();
    }

    public override void CheckHP(int oldValue, int newValue) {
        base.CheckHP(oldValue, newValue);

        if (_aiBossCharacter.IsOwner) {
            if (CurrentHealth.Value <= 0) { return; }
            float healthNeedeForChange = MaxHealth.Value * (_aiBossCharacter.Phase02ChangeHealthPercentage / 100);

            if (CurrentHealth.Value <= healthNeedeForChange) {
                _aiBossCharacter.Phase02Change();
            }
        }

    }
}
