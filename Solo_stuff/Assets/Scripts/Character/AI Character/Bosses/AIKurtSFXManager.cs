using UnityEngine;

public class AIKurtSFXManager : CharacterSFXManager
{
    [Header("Hammer Wooshes")]
    public AudioClip[] HammerWooshes;

    [Header("Impacts")]
    public AudioClip[] HammerImpact;
    public AudioClip[] FootImpact;

    public virtual void PlayHammerImpact() {
        if (HammerImpact.Length > 0) {
            PlaySoundFX(WorldSFXManager.Instance.ChooseRandomSFXFromArray(HammerImpact));
        }
    }

    public virtual void PlayFootImpact() {
        if (FootImpact.Length > 0) {
            PlaySoundFX(WorldSFXManager.Instance.ChooseRandomSFXFromArray(FootImpact));
        }
    }
}
