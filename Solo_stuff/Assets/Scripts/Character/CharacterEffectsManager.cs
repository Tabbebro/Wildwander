using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    CharacterManager _character;

    [Header("VFX")]
    [SerializeField] GameObject _bloodSplatterVFX;

    protected virtual void Awake() {
        _character = GetComponent<CharacterManager>();
    }

    public virtual void ProcessInstantEffect(InstantCharacterEffect effect) {
        effect.ProcessEffect(_character);
    }

    public void PlayBloodSplatterVFX(Vector2 contactPoint) {
        // For Manually Changing Blood Splatters Between Different Characters
        if (_bloodSplatterVFX != null) {
            GameObject bloodSplatter = Instantiate(_bloodSplatterVFX, contactPoint, Quaternion.identity);
        }
        // Use Default If Not Set Manually
        else {
            GameObject bloodSplatter = Instantiate(WorldCharacterEffectsManager.Instance.BloodSplatterVFX, contactPoint, Quaternion.identity);
        }
    }
}
