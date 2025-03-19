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

    [Header("Stamina Costs")]
    public int BaseStaminaCost = 20;

    // TODO: Add Defence

}
