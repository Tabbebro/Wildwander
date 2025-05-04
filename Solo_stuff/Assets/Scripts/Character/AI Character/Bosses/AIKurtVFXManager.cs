using System.Collections;
using UnityEngine;

public class AIKurtVFXManager : AICharacterVFXManager
{
    [Header("Stomp VFX")]
    public ParticleSystem StompVFX;

    public void PlayStompVFX(Vector3 location) {

        // Give Location
        StompVFX.transform.position = location;
        // Deparent
        StompVFX.transform.parent = null;
        // Enable GameObject
        StompVFX.gameObject.SetActive(true);
        // Play VFX
        StompVFX.Play();
        StartCoroutine(WaitForVFXToEnd(StompVFX));
    }
}
