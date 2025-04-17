using UnityEngine;
/// <summary>
/// Used For A Humanoid AI With Only Weapon In Right Hand
/// </summary>
public class AIHumanoidCombatManager : AICharacterCombatManager
{
    [Header("Damage Colliders")]
    [SerializeField] AIMeleeWeaponDamageCollider _WeaponDamageCollider;

    [Header("Damage")]
    [SerializeField] int _baseDamage = 25;
    [SerializeField] float _lightDamageModifier = 1.0f;
    [SerializeField] float _heavyDamageModifier = 1.5f;

    public void SetLightAttackDamage() {
        _WeaponDamageCollider.PhysicalDamage = _baseDamage * _lightDamageModifier;
    }

    public void SetHeavyAttackDamage() {
        _WeaponDamageCollider.PhysicalDamage = _baseDamage * _heavyDamageModifier;
    }

    public void OpenDamageCollider() {
        _WeaponDamageCollider.EnableDamageCollider();
    }

    public void CloseDamageCollider() { 
        _WeaponDamageCollider.DisableDamageCollider();
    }
}
