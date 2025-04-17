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
        // ADD Damage Grunts If Needed
        _aiCharacter.CharacterSoundFXManager.PlayAttackGrunt();
        _WeaponDamageCollider.EnableDamageCollider();
    }

    public void CloseDamageCollider() { 
        // ADD Damage Grunts If Needed
        _WeaponDamageCollider.DisableDamageCollider();
    }
}
