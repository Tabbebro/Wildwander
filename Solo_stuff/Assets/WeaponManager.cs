using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] MeleeWeaponDamageCollider _meleeDamageCollider;

    private void Awake() {
        _meleeDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
    }

    public void SetWeaponDamage(CharacterManager characterWieldingWeapon, WeaponItem weapon) {

        _meleeDamageCollider.CharacterCausingDamage = characterWieldingWeapon;
        _meleeDamageCollider.PhysicalDamage = weapon.PhysicalDamage;
        _meleeDamageCollider.MagicDamage = weapon.MagicDamage;
        _meleeDamageCollider.FireDamage = weapon.FireDamage;
        _meleeDamageCollider.LightningDamage = weapon.LightningDamage;
        _meleeDamageCollider.HolyDamage = weapon.HolyDamage;
    }
}
