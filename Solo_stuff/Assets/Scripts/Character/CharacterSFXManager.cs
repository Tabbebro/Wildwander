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
    public bool UseSurfaceFootsteps = true;
    public AudioClip[] FootstepsDefault;
    public AudioClip[] FootstepsDirt;
    public AudioClip[] FootstepsConcrete;
    public AudioClip[] FootstepsStone;
    public AudioClip[] FootstepsGravel;

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

    protected virtual void Footstep() {
        if (_character.IsDead.Value) { return; }
        if (_character.CharacterMovementManager.isRolling) { return; }
        if (_character.CharacterNetworkManager.IsJumping.Value) { return; }

        var footstep = _character.Animator.GetFloat("Footstep");

        if(Mathf.Abs(footstep) < 0.00001f) { footstep = 0; }

        if(_lastFootstep > 0 && footstep < 0 || _lastFootstep < 0 && footstep > 0) {

            _footstepAudioSource.PlayOneShot(WorldSFXManager.Instance.ChooseRandomSFXFromArray(GetFootstepClipFromSurface()));
        }

        _lastFootstep = footstep;
    }

    AudioClip[] GetFootstepClipFromSurface() {

        if (!UseSurfaceFootsteps) {
            if (FootstepsDefault.Length > 0) {
                return FootstepsDefault; 
            }
            else {
                return WorldSFXManager.Instance.FootstepsConcrete;
            }
        }

        RaycastHit hit;

        Physics.Raycast(transform.position, Vector3.down, out hit, WorldUtilityManager.Instance.GetEnviroLayers());

        if (hit.transform.GetComponent<Utility_SurfaceType>() != null) {

            SurfaceType surface = hit.transform.GetComponent<Utility_SurfaceType>().SurfaceType;

            // TODO: ADD more surfaces
            switch (surface) {
                case SurfaceType.Dirt:
                    print("Dirt");
                    if (FootstepsDirt.Length > 0) {
                        return FootstepsDirt;
                    }
                    else {
                        return WorldSFXManager.Instance.FootstepsDirt;
                    }
                case SurfaceType.Concrete:
                    print("Concrete");
                    if (FootstepsConcrete.Length > 0) {
                        return FootstepsConcrete;
                    }
                    else {
                        return WorldSFXManager.Instance.FootstepsConcrete;
                    }
                case SurfaceType.Gravel:
                    print("Gravel");
                    if (FootstepsGravel.Length > 0) {
                        return FootstepsGravel;
                    }
                    else {
                        return WorldSFXManager.Instance.FootstepsGravel;
                    }
                case SurfaceType.Stone:
                    print("Stone");
                    if (FootstepsStone.Length > 0) {
                        return FootstepsStone;
                    }
                    else {
                        return WorldSFXManager.Instance.FootstepsStone;
                    }
            }
        }

        return WorldSFXManager.Instance.FootstepsConcrete;
    }
}
