using Unity.VisualScripting;
using UnityEngine;

public class CharacterSoundFXManager : MonoBehaviour
{
    private AudioSource _audioSource;
    [Header("Damage Grunts")]
    [SerializeField] protected AudioClip[] _damageGrunts;

    [Header("Attack Grunts")]
    [SerializeField] protected AudioClip[] _attackGrunts;
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

    public void PlayRollSoundFX() {
        _audioSource.PlayOneShot(WorldSoundFXManager.Instance.RollSFX);
    }

    public virtual void PlayDamageGrunt() {
        if (_damageGrunts.Length > 0) {
            PlaySoundFX(WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(_damageGrunts));
        }
    }

    public virtual void PlayAttackGrunt() {
        if (_attackGrunts.Length > 0) {
            PlaySoundFX(WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(_attackGrunts));
        }
    }
}
