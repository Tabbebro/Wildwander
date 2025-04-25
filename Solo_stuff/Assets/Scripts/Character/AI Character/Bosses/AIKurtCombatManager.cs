using System.Collections.Generic;
using UnityEngine;

public class AIKurtCombatManager : AICharacterCombatManager
{
    AIKurtCharacterManager _kurtManager;

    [Header("Damage Colliders")]
    [SerializeField] KurtWeaponDamageCollider _WeaponDamageCollider;
    [SerializeField] KurtStompCollider _StompDamageCollider;
    [SerializeField] Transform _stompFoot;
    public float StompRadius = 1.5f;

    [Header("Damage")]
    [SerializeField] int _baseDamage = 25;
    [SerializeField] float _attack01DamageModifier = 1.0f;
    [SerializeField] float _attack02DamageModifier = 1.4f;
    [SerializeField] float _attack03DamageModifier = 1.6f;
    public float StompDamage = 25f;

    [Header("VFX")]
    public GameObject ImpactVFX;

    protected override void Awake() {
        base.Awake();

        _kurtManager = GetComponent<AIKurtCharacterManager>();
    }

    public void SetAttack01Damage() {
        _aiCharacter.CharacterSFXManager.PlayAttackGruntSFX();
        _WeaponDamageCollider.PhysicalDamage = _baseDamage * _attack01DamageModifier;
    }

    public void SetAttack02Damage() {
        _aiCharacter.CharacterSFXManager.PlayAttackGruntSFX();
        _WeaponDamageCollider.PhysicalDamage = _baseDamage * _attack02DamageModifier;
    }

    public void SetAttack03Damage() {
        _aiCharacter.CharacterSFXManager.PlayAttackGruntSFX();
        _WeaponDamageCollider.PhysicalDamage = _baseDamage * _attack03DamageModifier;
    }

    public void OpenClubDamageCollider() {
        _kurtManager.CharacterSFXManager.PlaySoundFX(WorldSFXManager.Instance.ChooseRandomSFXFromArray(_kurtManager.KurtSoundManager.HammerWooshes));
        _WeaponDamageCollider.EnableDamageCollider();
    }

    public void CloseClubDamageCollider() {
        _WeaponDamageCollider.DisableDamageCollider();
    }

    public void ActivateStomp() {
        _StompDamageCollider.StompAttack();
    }

    public override void PivotTowardsTarget(AICharacterManager aiCharacter) {
        // Play Correct Pivot Animation
        if (aiCharacter.IsPerformingAction) { return; }


        // 90 Angle Pivot
        if (ViewableAngle >= 61 && ViewableAngle <= 110) {
            aiCharacter.CharacterAnimatorManager.PlayTargetActionAnimation("Turn_R90", true);
        }
        else if (ViewableAngle <= -61 && ViewableAngle >= -110) {
            aiCharacter.CharacterAnimatorManager.PlayTargetActionAnimation("Turn_L90", true);
        }

        // 180 Angle Pivot
        else if (ViewableAngle >= 146 && ViewableAngle <= 180) {
            aiCharacter.CharacterAnimatorManager.PlayTargetActionAnimation("Turn_L180", true);
        }
        else if (ViewableAngle <= -146 && ViewableAngle >= -180) {
            aiCharacter.CharacterAnimatorManager.PlayTargetActionAnimation("Turn_L180", true);
        }
    }
}
