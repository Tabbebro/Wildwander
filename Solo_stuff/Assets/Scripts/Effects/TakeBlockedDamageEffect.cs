using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Blocked Damage")]
public class TakeBlockedDamageEffect : InstantCharacterEffect
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

        if (character.CharacterNetworkManager.IsInvulnerable.Value) { return; }

        base.ProcessEffect(character);

        // If Character Is Dead Return
        if (character.IsDead.Value) { return; }

        Debug.Log("Hit Was Blocked");

        // Check For I Frames

        // Calculate DMG
        CalculateDamage(character);

        // Play Animation Based On Direction
        PlayDirectionalBasedBlockingDamageAnimation(character);

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

        Debug.Log("Original Physical Damage: " + PhysicalDamage);

        PhysicalDamage -= (PhysicalDamage * (character.CharacterStatsManager.BlockingPhysicalAbsorption / 100));
        MagicDamage -= (MagicDamage * (character.CharacterStatsManager.BlockingMagicAbsorption / 100));
        FireDamage -= (FireDamage * (character.CharacterStatsManager.BlockingFireAbsorption / 100));
        LightningDamage -= (LightningDamage * (character.CharacterStatsManager.BlockingLightningAbsorption / 100));
        HolyDamage -= (HolyDamage * (character.CharacterStatsManager.BlockingHolyAbsorption / 100));
        

        _finalDamage = Mathf.RoundToInt(PhysicalDamage + MagicDamage + FireDamage + LightningDamage + HolyDamage);

        if (_finalDamage <= 0) {
            _finalDamage = 1;
        }

        Debug.Log("Physical Damage After Absorption: " + PhysicalDamage);

        character.CharacterNetworkManager.CurrentHealth.Value -= _finalDamage;

        // TODO: Calculate Poise Damage
    }

    void PlayDamageVFX(CharacterManager character) {
        // TODO: Add Elemental VFX

        // TODO: Get VFX Based On Blocking Weapon
    }

    void PlayDamageSFX(CharacterManager character) {
        // TODO: Add Elemental SFX

        // TODO: Get SFX Based On Blocking Weapon
    }

    void PlayDirectionalBasedBlockingDamageAnimation(CharacterManager character) {

        if (!character.IsOwner) { return; }

        if (character.IsDead.Value) { return; }

        DamageIntensity damageIntensity = WorldUtilityManager.Instance.GetDamageIntensityBasedOnPoiseDamage(PoiseDamage);

        // TODO: Check For Two Hand Status
        switch (damageIntensity) {
            case DamageIntensity.Ping:
                DamageAnimation = "Block_Ping_01";
                break;
            case DamageIntensity.Light:
                DamageAnimation = "Block_Light_01";
                break;
            case DamageIntensity.Medium:
                DamageAnimation = "Block_Medium_01";
                break;
            case DamageIntensity.Heavy:
                DamageAnimation = "Block_Heavy_01";
                break;
            case DamageIntensity.Colossal:
                DamageAnimation = "Block_Colossal_01";
                break;
            default:
                break;
        }

        character.CharacterAnimatorManager.LastDamageAnimationPlayed = DamageAnimation;
        character.CharacterAnimatorManager.PlayTargetActionAnimation(DamageAnimation, true);
    }
}
