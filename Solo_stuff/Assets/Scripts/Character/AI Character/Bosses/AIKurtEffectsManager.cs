using UnityEngine;

public class AIKurtEffectsManager : AICharacterEffectsManager
{
    [Header("Stomp VFX")]
    [SerializeField] ParticleSystem _stompVFX;

    public void PlayStompVFX(Vector3 location) {
        StartCoroutine(PlayVFX(_stompVFX, _vfxTransform, location));
    }
}
