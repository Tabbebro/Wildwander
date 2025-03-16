using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    CharacterManager _character;

    protected virtual void Awake() {
        _character = GetComponent<CharacterManager>();
    }

    public virtual void ProcessInstantEffect(InstantCharacterEffect effect) {
        effect.ProcessEffect(_character);
    }
}
