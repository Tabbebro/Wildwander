using UnityEngine;

public class PlayerEffectsManager : CharacterEffectsManager
{
    [Header("Debug")]
    [SerializeField] InstantCharacterEffect _effectToTest;
    [SerializeField] bool _processEffect = false;

    private void Update() {
        if (_processEffect) {
            _processEffect = false;
            TakeStaminaDamageEffect effect = Instantiate(_effectToTest) as TakeStaminaDamageEffect;
            effect.StaminaDamage = 55;
            ProcessInstantEffect(effect);
        }
    }
}
