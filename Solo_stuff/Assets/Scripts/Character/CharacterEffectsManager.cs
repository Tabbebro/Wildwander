using UnityEngine;
using System.Collections;
using static UnityEngine.ParticleSystem;

public class CharacterEffectsManager : MonoBehaviour
{
    CharacterManager _character;

    [Header("VFX Parent")]
    [SerializeField] protected Transform _vfxTransform;

    [Header("Blood Splatter")]
    [SerializeField] ParticleSystem _bloodSplatterVFX;

    [Header("Dust Trails")]
    [SerializeField] Vector3 _dustTrailPositionOffset;
    [SerializeField] Transform _rightLeg;
    [SerializeField] Transform _leftLeg;
    [SerializeField] ParticleSystem _walkDustTrail;
    [SerializeField] ParticleSystem _sprintDustTrail;

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
            //GameObject bloodSplatter = Instantiate(_bloodSplatterVFX, contactPoint, Quaternion.identity);
            StartCoroutine(PlayVFX(_bloodSplatterVFX, _vfxTransform, contactPoint));
        }
        // Use Default If Not Set Manually
        else {
            GameObject bloodSplatter = Instantiate(WorldCharacterEffectsManager.Instance.BloodSplatterVFX, contactPoint, Quaternion.identity);
        }
    }

    public void PlayDustTrailVFX(bool isSprinting) {
        if (_rightLeg == null || _leftLeg == null) { return; }

        ParticleSystem dustParticle = null;
        if (_character.CharacterNetworkManager.IsSprinting.Value) {
            dustParticle = _sprintDustTrail;
        }
        else {
            dustParticle = _walkDustTrail;
        }

        if (_rightLeg.position.y < _leftLeg.position.y) { // Right Leg Lower
            StartCoroutine(PlayVFX(dustParticle, _vfxTransform, _rightLeg.transform.position + _dustTrailPositionOffset));
        }
        else if (_leftLeg.position.y < _rightLeg.position.y) { // Left Leg Lower
            StartCoroutine(PlayVFX(dustParticle, _vfxTransform, _leftLeg.transform.position + _dustTrailPositionOffset));
        }

    }
}
