using UnityEngine;

public class AICharacterEffectsManager : CharacterEffectsManager 
{
    [Header("Stomp VFX")]
    [SerializeField] ParticleSystem _stompVFX;

    public void PlayStompVFX(Vector3 location) {
        StartCoroutine(PlayVFX(_stompVFX, _vfxTransform, location));
    }
}
