using UnityEngine;
using System.Collections;

public class CharacterVFXManager : MonoBehaviour
{
    [Header("VFX Parent Transform")]
    [SerializeField] protected Transform VFXParent;
    protected virtual void Awake() {

    }

    protected IEnumerator WaitForVFXToEnd(ParticleSystem particle) {
        while (particle.isPlaying) {
            yield return null;
        }
        // Disable GameObject
        particle.gameObject.SetActive(false);
        // Reparent VFX
        particle.transform.SetParent(VFXParent);
    }
}
