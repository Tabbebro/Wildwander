using UnityEngine;

public class CharacterSFXManager : MonoBehaviour
{
    CharacterManager _character;
    AudioSource _audioSource;
    
    [Header("Damage Grunts")]
    [SerializeField] protected AudioClip[] _damageGrunts;

    [Header("Attack Grunts")]
    [SerializeField] protected AudioClip[] _attackGrunts;

    [Header("Footsteps overrides")]
    [SerializeField] AudioSource _footstepAudioSource;
    float _lastFootstep;
    [SerializeField] protected bool _useCustomFootsteps = false;
    [SerializeField] protected AudioClip[] _footstepCustomWalk;
    [SerializeField] protected AudioClip[] _footstepCustomRun;
    [SerializeField] protected AudioClip[] _footstepCustomSprint;


    protected virtual void Awake() {
        _character = GetComponent<CharacterManager>();
        _audioSource = GetComponent<AudioSource>();
    }

    protected virtual void FixedUpdate() {
        Footstep();
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
    /// <summary>
    /// Checks Animator Float "Footstep" To See If Footstep Sound Should Be Played
    /// </summary>
    protected virtual void Footstep() {
        if (_character.IsDead.Value) { return; }
        if (_character.IsPerformingAction) { return; }
        if (_character.CharacterMovementManager.isRolling) { return; }
        if (!_character.CharacterNetworkManager.IsMoving.Value) { return; }
        if (_character.CharacterNetworkManager.IsJumping.Value) { return; }

        var footstep = _character.Animator.GetFloat("Footstep");

        if(Mathf.Abs(footstep) < 0.00001f) { footstep = 0; }

        if (_lastFootstep < 0 && footstep > 0) { // Right Foot
            // Play Audio
            _footstepAudioSource.PlayOneShot(WorldSFXManager.Instance.ChooseRandomSFXFromArray(GetFootstepClipFromSurface()));
            // Play DustTrail VFX
            _character.CharacterEffectsManager.PlayDustTrailVFX(_character.CharacterNetworkManager.IsSprinting.Value, _character.CharacterEffectsManager.RightFootTransform.position);
        }
        else if (_lastFootstep > 0 && footstep < 0) { // Left Foot
            // Play Audio
            _footstepAudioSource.PlayOneShot(WorldSFXManager.Instance.ChooseRandomSFXFromArray(GetFootstepClipFromSurface()));
            // Play DustTrail VFX
            _character.CharacterEffectsManager.PlayDustTrailVFX(_character.CharacterNetworkManager.IsSprinting.Value, _character.CharacterEffectsManager.LeftFootTransform.position);
        }

        _lastFootstep = footstep;
    }

    /// <summary>
    /// Shoots Ray Downward Towards Ground & Checks If There Is A Utility_SurfaceType Script & Checks Ground Type From That
    /// </summary>
    AudioClip[] GetFootstepClipFromSurface() {

        if (_useCustomFootsteps) {
            return GetCustomFootstepSound(_character.CharacterNetworkManager.MoveAmount.Value, _character.CharacterNetworkManager.IsSprinting.Value);
        }

        RaycastHit hit;

        Physics.Raycast(transform.position, Vector3.down, out hit, WorldUtilityManager.Instance.GetEnviroLayers());

        if (hit.transform.GetComponent<Utility_SurfaceType>() != null) {

            SurfaceType surface = hit.transform.GetComponent<Utility_SurfaceType>().SurfaceType;

            return GetFootstepSound(surface, _character.CharacterNetworkManager.MoveAmount.Value, _character.CharacterNetworkManager.IsSprinting.Value);
        }

        return GetFootstepSound(SurfaceType.Untagged, _character.CharacterNetworkManager.MoveAmount.Value, _character.CharacterNetworkManager.IsSprinting.Value);
    }

    /// <summary>
    /// Returns Correct Footstep sound from SurfaceType & Movement Speed
    /// </summary>
    AudioClip[] GetFootstepSound(SurfaceType surface, float speed, bool isSprinting) {
        // TODO: ADD more surfaces
        switch (surface) {
            case SurfaceType.Dirt:
                if (speed == 1 && isSprinting) { // Sprint
                    return WorldSFXManager.Instance.FootstepsDirtSprint;
                }
                else if (speed == 1) { // Run
                    return WorldSFXManager.Instance.FootstepsDirtRun;
                }
                else { // Walk
                    return WorldSFXManager.Instance.FootstepsDirtWalk;
                }

            case SurfaceType.Concrete:
                if (speed == 1 && isSprinting) { // Sprint
                    return WorldSFXManager.Instance.FootstepsConcreteSprint;
                }
                else if (speed == 1) { // Run
                    return WorldSFXManager.Instance.FootstepsConcreteRun;
                }
                else { // Walk
                    return WorldSFXManager.Instance.FootstepsConcreteWalk;
                }

            case SurfaceType.Gravel:
                if (speed == 1 && isSprinting) { // Sprint
                    return WorldSFXManager.Instance.FootstepsGravelSprint;
                }
                else if (speed == 1) { // Run
                    return WorldSFXManager.Instance.FootstepsGravelRun;
                }
                else { // Walk
                    return WorldSFXManager.Instance.FootstepsGravelWalk;
                }

            case SurfaceType.Stone:
                if (speed == 1 && isSprinting) { // Sprint
                    return WorldSFXManager.Instance.FootstepsStoneSprint;
                }
                else if (speed == 1) { // Run
                    return WorldSFXManager.Instance.FootstepsStoneRun;
                }
                else { // Walk
                    return WorldSFXManager.Instance.FootstepsStoneWalk;
                }

            default: // Works as the "Untagged" SurfaceType
                if (speed == 1 && isSprinting) { // Sprint
                    return WorldSFXManager.Instance.FootstepsConcreteSprint;
                }
                else if (speed == 1) { // Run
                    return WorldSFXManager.Instance.FootstepsConcreteRun;
                }
                else { // Walk
                    return WorldSFXManager.Instance.FootstepsConcreteWalk;
                }
        }

    }

    /// <summary>
    /// Returns Custom Footstep Sound Using Movement Speed
    /// </summary>
    AudioClip[] GetCustomFootstepSound(float speed, bool isSprinting) {
        if (speed == 1 && isSprinting) { // Sprint
            return _footstepCustomSprint;
        }
        else if (speed == 1) { // Run
            return _footstepCustomRun;
        }
        else { // Walk
            return _footstepCustomWalk;
        }
    }
}
