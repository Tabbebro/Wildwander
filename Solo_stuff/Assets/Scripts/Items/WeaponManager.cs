using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public MeleeWeaponDamageCollider MeleeDamageCollider;

    private void Awake() {
        MeleeDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
    }

    public void SetWeaponDamage(CharacterManager characterWieldingWeapon, WeaponItem weapon) {

        MeleeDamageCollider.CharacterCausingDamage = characterWieldingWeapon;
        MeleeDamageCollider.PhysicalDamage = weapon.PhysicalDamage;
        MeleeDamageCollider.MagicDamage = weapon.MagicDamage;
        MeleeDamageCollider.FireDamage = weapon.FireDamage;
        MeleeDamageCollider.LightningDamage = weapon.LightningDamage;
        MeleeDamageCollider.HolyDamage = weapon.HolyDamage;

        // Light Attack
        MeleeDamageCollider.LightAttack01Modifier = weapon.LightAttack01Modifier;
        MeleeDamageCollider.LightAttack02Modifier = weapon.LightAttack02Modifier;

        // Melee Weapon
        MeleeDamageCollider.HeavyAttack01Modifier = weapon.HeavyAttack01Modifier;
        MeleeDamageCollider.HeavyAttack02Modifier = weapon.HeavyAttack02Modifier;
        MeleeDamageCollider.HeavyAttackHold01Modifier = weapon.HeavyAttackHold01Modifier;
        MeleeDamageCollider.HeavyAttackHold02Modifier = weapon.HeavyAttackHold02Modifier;
    }
}
