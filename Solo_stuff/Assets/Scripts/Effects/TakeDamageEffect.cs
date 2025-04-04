using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Damage")]
public class TakeDamageEffect : InstantCharacterEffect
{
    [Header("Character Causing Damage")]
    public CharacterManager CharacterCausingDamage; // Character That Deals Damage

    [Header("Damage")]
    public float PhysicalDamage = 0; // TODO: Split Into Slashing, Piercing & Bludgeoning
    public float MagicDamage = 0;
    public float FireDamage = 0;
    public float LightningDamage = 0;
    public float HolyDamage = 0;
    // TODO: Make More Damage Types

    // Build Ups
    // TODO: Add Build Ups

    [Header("Final Damage")]
    int _finalDamage = 0; // Damage Dealt After All Calculations

    [Header("Poise")]
    public float PoiseDamage = 0;
    public bool PoiseIsBroken = false;

    [Header("Animation")]
    public bool PlayeDamageAnimation = true;
    public bool ManuallySelectDamageAnimation = false;
    public string DamageAnimation;

    [Header("Sound FX")]
    public bool WillPlayDamageSFX = true;
    public AudioClip ElementalDamageSoundFX;

    [Header("Direction Damage Taken From")]
    public float AngleHitFrom; // Direction Where Hit Came From For Animations
    public Vector3 ContactPoint; // Point Where Hit Happened For Blood Instantiation


    public override void ProcessEffect(CharacterManager character) {
        base.ProcessEffect(character);

        // If Character Is Dead Return
        if (character.IsDead.Value) { return; }

        // Check For I Frames

        // Calculate DMG
        CalculateDamage(character);

        // Check DMG Direction

        // Play Animation
        PlayDirectionalBadsedDamageAnimation(character);
        // Check For Build Ups

        // Play SFX
        PlayDamageSFX(character);
        // Play VFX
        PlayDamageVFX(character);

    }

    void CalculateDamage(CharacterManager character) {

        if (!character.IsOwner) { return; }

        if (CharacterCausingDamage != null) {
            // TODO: Check For Damage Modifiers
        }

        // TODO: Check For Defences / Absorptions

        _finalDamage = Mathf.RoundToInt(PhysicalDamage + MagicDamage + FireDamage + LightningDamage + HolyDamage);

        if (_finalDamage <= 0) {
            _finalDamage = 1;
        }

        character.CharacterNetworkManager.CurrentHealth.Value -= _finalDamage;

        // TODO: Calculate Poise Damage
    }

    void PlayDamageVFX(CharacterManager character) {
        // TODO: Add Elemental VFX

        character.CharacterEffectsManager.PlayBloodSplatterVFX(ContactPoint);
    }

    void PlayDamageSFX(CharacterManager character) {
        AudioClip physicalDamageSFX = WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(WorldSoundFXManager.Instance.PhysicalDamageSFX);
        Debug.Log("Audio Played");
        character.CharacterSoundFXManager.PlaySoundFX(physicalDamageSFX);
        // TODO: Add Elemental SFX
    }

    void PlayDirectionalBadsedDamageAnimation(CharacterManager character) {
        
        if(!character.IsOwner) { return; } 

        // TODO: Calculate If Poise Breaks
        PoiseIsBroken = true;
        
        // Front
        if (AngleHitFrom >= 145 && AngleHitFrom <= 180) {
            DamageAnimation = character.CharacterAnimatorManager.GetRandomAnimationFromList(character.CharacterAnimatorManager.front_Medium_Damage);
        }
        // Front
        else if (AngleHitFrom <= -145 && AngleHitFrom >= -180) {
            DamageAnimation = character.CharacterAnimatorManager.GetRandomAnimationFromList(character.CharacterAnimatorManager.front_Medium_Damage);
        }
        // Back
        else if (AngleHitFrom >= -45 && AngleHitFrom <= 45) {
            DamageAnimation = character.CharacterAnimatorManager.GetRandomAnimationFromList(character.CharacterAnimatorManager.back_Medium_Damage);
        }
        // Left
        else if (AngleHitFrom >= -144 && AngleHitFrom <= -45) {
            DamageAnimation = character.CharacterAnimatorManager.GetRandomAnimationFromList(character.CharacterAnimatorManager.left_Medium_Damage);
        }
        // Right
        else if (AngleHitFrom >= 45 && AngleHitFrom <= 144) {
            DamageAnimation = character.CharacterAnimatorManager.GetRandomAnimationFromList(character.CharacterAnimatorManager.right_Medium_Damage);
        }

        if (PoiseIsBroken) {
            Debug.Log("Animation Played: " + DamageAnimation);
            character.CharacterAnimatorManager.LastDamageAnimationPlayed = DamageAnimation; 
            character.CharacterAnimatorManager.PlayTargetActionAnimation(DamageAnimation, true);
        }
    }
}
