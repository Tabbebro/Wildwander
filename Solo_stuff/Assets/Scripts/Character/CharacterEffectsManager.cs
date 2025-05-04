using UnityEngine;
using System.Collections;

public class CharacterEffectsManager : MonoBehaviour
{
    CharacterManager _character;

    [Header("VFX Parent")]
    [SerializeField] protected Transform _vfxTransform;

    [Header("VFX")]
    [SerializeField] GameObject _bloodSplatterVFX;

    protected virtual void Awake() {
        _character = GetComponent<CharacterManager>();
    }

    public virtual void ProcessInstantEffect(InstantCharacterEffect effect) {
        effect.ProcessEffect(_character);
    }

    protected IEnumerator PlayVFX(ParticleSystem particle, Transform vfxParent, Vector3 location) {
        // Give Location
        particle.transform.position = location;
        // Deparent
        particle.transform.parent = null;
        // Enable GameObject
        particle.gameObject.SetActive(true);
        // Play VFX
        particle.Play();

        while (particle.isPlaying) {
            yield return null;
        }
        // Disable GameObject
        particle.gameObject.SetActive(false);
        // Reparent VFX
        particle.transform.SetParent(vfxParent);
    }

    public void PlayBloodSplatterVFX(Vector3 contactPoint) {

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
