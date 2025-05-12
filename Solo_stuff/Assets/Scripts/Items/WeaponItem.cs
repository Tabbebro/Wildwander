using UnityEngine;

public class WeaponItem : Item
{
    [Header("Weapon Model")]
    public GameObject weaponModel;

    [Header("WeaponRequirements")]
    public int StrReq = 0;
    public int DexReq = 0;
    public int IntReq = 0;
    public int FaiReq = 0;

    [Header("Weapon Base Damage")]
    public int PhysicalDamage = 0;
    public int MagicDamage = 0;
    public int FireDamage = 0;
    public int LightningDamage = 0;
    public int HolyDamage = 0;

    [Header("Weapon Poise")]
    public float PoiseDamage = 10;
    // TODO: Add Hyper Armor

    [Header("Attack Modifiers")]
    // TODO: Add Modifiers
    public float LightAttack01Modifier = 1f;
    public float LightAttack02Modifier = 1.2f;
    public float HeavyAttack01Modifier = 1.5f;
    public float HeavyAttack02Modifier = 1.7f;
    public float HeavyAttackHold01Modifier = 2.0f;
    public float HeavyAttackHold02Modifier = 2.2f;
    public float RunAttack01Modifier = 1.1f;
    public float RollAttack01Modifier = 1.1f;
    public float BackstepAttack01Modifier = 1.1f;

    [Header("Stamina Costs")]
    public int BaseStaminaCost = 20;
    public float LigthAttackStaminaModifier = 1f;
    public float HeavyAttackStaminaModifier = 1.25f;
    public float HeavyAttackHoldStaminaModifier = 1.5f;
    public float RunAttackStaminaModifier = 1.1f;
    public float RollAttackStaminaModifier = 1.1f;
    public float BackstepAttackStaminaModifier = 1.1f;


    // TODO: Add Defence

    [Header("Actions")]
    public WeaponItemAction OhLightAction; // One Handed Light Action
    public WeaponItemAction OhHeavyAction; // One Handed Heavy Action

    [Header("Sound FX")]
    public AudioClip[] SwooshSFXs;
}
