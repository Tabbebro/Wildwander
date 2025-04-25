using UnityEngine;

public class CharacterFootstepManager : MonoBehaviour
{
    CharacterManager _character;

    AudioSource _audioSource;
    GameObject _steppedOnObject;

    bool _hasTouchedGround = false;
    bool _hasPlayedFootstepSFX = false;

    [SerializeField] float _distanceToGround = 0.05f;

    private void Awake() {
        _audioSource = GetComponent<AudioSource>();

        _character = GetComponentInParent<CharacterManager>();
    }

    private void FixedUpdate() {
        CheckForFootstep();
    }
    void CheckForFootstep() {
        if (_character == null) { return; }

        if (!_character.CharacterNetworkManager.IsMoving.Value) { return; }

        RaycastHit hit;

        if (Physics.Raycast(transform.position, _character.transform.TransformDirection(Vector3.down), out hit, _distanceToGround, WorldUtilityManager.Instance.GetEnviroLayers())) {
            _hasTouchedGround = true;

            if (!_hasPlayedFootstepSFX) {
                _steppedOnObject = hit.transform.gameObject;
            }
        }
        else { 
            _hasTouchedGround = false;
            _hasPlayedFootstepSFX = false;
            _steppedOnObject = null;
        }

        if (_hasTouchedGround && !_hasPlayedFootstepSFX) {
            _hasPlayedFootstepSFX = true;

            PlayFootstepSFX();
        }
    }

    void PlayFootstepSFX() {
        //_audioSource.PlayOneShot(WorldSFXManager.Instance.ChooseRandomFootstepBasedOnGround(_steppedOnObject, _character));

        _character.CharacterSFXManager.PlayFootstepSFX();
    }
}
