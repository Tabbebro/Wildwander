using System.Collections.Generic;
using UnityEngine;

public class WorldCharacterEffectsManager : MonoBehaviour
{
    public static WorldCharacterEffectsManager Instance;

    [Header("VFX")]
    public GameObject BloodSplatterVFX;

    [Header("Damage")]
    public TakeDamageEffect TakeDamageEffect;

    [SerializeField] List<InstantCharacterEffect> _instantEffects;

    private void Awake() {

        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }

        GenerateEffectIDs();
    }

    private void GenerateEffectIDs() {
        for (int i = 0; i < _instantEffects.Count; i++) {
            _instantEffects[i].InstantEffectID = i;
        }
    }
}
