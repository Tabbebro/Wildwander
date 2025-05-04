using System.Collections.Generic;
using UnityEngine;

public class AIKurtCombatManager : AICharacterCombatManager
{
    AIKurtCharacterManager _kurtManager;

    [Header("Damage Colliders")]
    [SerializeField] KurtWeaponDamageCollider _weaponDamageCollider;
    [SerializeField] KurtStompCollider _stompDamageCollider;

    [Header("Damage")]
    [SerializeField] int _baseDamage = 25;
    [SerializeField] float _attack01DamageModifier = 1.0f;
    [SerializeField] float _attack02DamageModifier = 1.4f;
    [SerializeField] float _attack03DamageModifier = 1.6f;
    [SerializeField] float _stompDamage = 25f;

    [Header("VFX")]
    public GameObject ImpactVFX;

    protected override void Awake() {
        base.Awake();

        _kurtManager = GetComponent<AIKurtCharacterManager>();
    }

    public void SetAttack01Damage() {
        _aiCharacter.CharacterSFXManager.PlayAttackGruntSFX();
        _weaponDamageCollider.PhysicalDamage = _baseDamage * _attack01DamageModifier;
    }

    public void SetAttack02Damage() {
        _aiCharacter.CharacterSFXManager.PlayAttackGruntSFX();
        _weaponDamageCollider.PhysicalDamage = _baseDamage * _attack02DamageModifier;
    }

    public void SetAttack03Damage() {
        _aiCharacter.CharacterSFXManager.PlayAttackGruntSFX();
        _stompDamageCollider.PhysicalDamage = _stompDamage;
        _weaponDamageCollider.PhysicalDamage = _baseDamage * _attack03DamageModifier;
    }

    public void OpenClubDamageCollider() {
        _kurtManager.CharacterSFXManager.PlaySoundFX(WorldSFXManager.Instance.ChooseRandomSFXFromArray(_kurtManager.KurtSoundManager.HammerWooshes));
        _weaponDamageCollider.EnableDamageCollider();
    }

    public void CloseClubDamageCollider() {
        _weaponDamageCollider.DisableDamageCollider();
    }

    public void OpenStompDamageCollider() {
        _stompDamageCollider.EnableDamageCollider();
    }

    public void CloseStompDamageCollider() {
        _stompDamageCollider.DisableDamageCollider();
    }

    public void StompUtility() {
        _stompDamageCollider.PlayVFX();
        _stompDamageCollider.PlaySFX();
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
