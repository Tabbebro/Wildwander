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

        MeleeDamageCollider.LightAttack01Modifier = weapon.LightAttack01Modifier;
        MeleeDamageCollider.HeavyAttack01Modifier = weapon.HeavyAttack01Modifier;
        MeleeDamageCollider.HeavyAttackHold01Modifier = weapon.HeavyAttackHold01Modifier;
    }
}
