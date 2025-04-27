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
}
