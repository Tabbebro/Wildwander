using Unity.VisualScripting;
using UnityEngine;

public class CharacterSFXManager : MonoBehaviour
{
    private AudioSource _audioSource;
    [Header("Damage Grunts")]
    [SerializeField] protected AudioClip[] _damageGrunts;

    [Header("Attack Grunts")]
    [SerializeField] protected AudioClip[] _attackGrunts;

    [Header("Footsteps")]
    public AudioClip[] FootstepsDirt;
    //public AudioClip[] FootstepsConcrete;
    //public AudioClip[] FootstepsStone;
    //public AudioClip[] FootstepsGravel;

    protected virtual void Awake() {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundFX(AudioClip soundFX, float volume = 1, bool randomizePitch = true, float pitchRandom = 0.1f) {
        _audioSource.PlayOneShot(soundFX, volume);
        // Reset Pitch
        _audioSource.pitch = 1;
        if (randomizePitch) {
            _audioSource.pitch += Random.Range(-pitchRandom, pitchRandom);
        }
    }

    public void PlayRollSFX() {
        _audioSource.PlayOneShot(WorldSFXManager.Instance.RollSFX);
    }

    public virtual void PlayDamageGruntSFX() {
        if (_damageGrunts.Length > 0) {
            PlaySoundFX(WorldSFXManager.Instance.ChooseRandomSFXFromArray(_damageGrunts));
        }
    }

    public virtual void PlayAttackGruntSFX() {
        if (_attackGrunts.Length > 0) {
            PlaySoundFX(WorldSFXManager.Instance.ChooseRandomSFXFromArray(_attackGrunts));
        }
    }

    public virtual void PlayFootstepSFX() {
        if (FootstepsDirt.Length > 0) {
            PlaySoundFX(WorldSFXManager.Instance.ChooseRandomSFXFromArray(FootstepsDirt));
        }
    }
}
