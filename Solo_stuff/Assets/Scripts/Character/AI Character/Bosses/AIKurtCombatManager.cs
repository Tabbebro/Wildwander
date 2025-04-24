using System.Collections.Generic;
using UnityEngine;

public class AIKurtCombatManager : AICharacterCombatManager
{
    [Header("Damage Colliders")]
    [SerializeField] KurtWeaponDamageCollider _WeaponDamageCollider;
    [SerializeField] KurtWeaponDamageCollider _StompDamageCollider;
    [SerializeField] Transform _stompFoot;
    [SerializeField] float _stompRadius = 1.5f;

    [Header("Damage")]
    [SerializeField] int _baseDamage = 25;
    [SerializeField] float _attack01DamageModifier = 1.0f;
    [SerializeField] float _attack02DamageModifier = 1.4f;
    [SerializeField] float _attack03DamageModifier = 1.6f;
    [SerializeField] float _stompDamage = 25f;

    public void SetAttack01Damage() {
        _WeaponDamageCollider.PhysicalDamage = _baseDamage * _attack01DamageModifier;
    }

    public void SetAttack02Damage() {
        _WeaponDamageCollider.PhysicalDamage = _baseDamage * _attack02DamageModifier;
    }

    public void SetAttack03Damage() {
        _WeaponDamageCollider.PhysicalDamage = _baseDamage * _attack03DamageModifier;
    }

    public void SetStompAttackDamage() {
        _StompDamageCollider.PhysicalDamage = _stompDamage;
    }

    public void OpenClubDamageCollider() {
        _aiCharacter.CharacterSoundFXManager.PlayAttackGrunt();
        _WeaponDamageCollider.EnableDamageCollider();
    }

    public void CloseClubDamageCollider() {
        _WeaponDamageCollider.DisableDamageCollider();
    }

    public void OpenStompDamageCollider() {
        _aiCharacter.CharacterSoundFXManager.PlayAttackGrunt();
        _StompDamageCollider.EnableDamageCollider();
    }

    public void CloseStompDamageCollider() {
        _StompDamageCollider.DisableDamageCollider();
    }

    public void ActivateKurtStomp() {
        Collider[] colliders = Physics.OverlapSphere(_stompFoot.position, _stompRadius, WorldUtilityManager.Instance.GetCharacterLayers());
        List<CharacterManager> _charactersDamaged = new();

        foreach (Collider collider in colliders) {
            CharacterManager character = collider.GetComponentInParent<CharacterManager>();

            if (character != null) {
                
                if (_charactersDamaged.Contains(character)) { continue; }

                _charactersDamaged.Add(character);

                if (character.IsOwner) {
                    // TODO: Check For Block

                    TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Instance.TakeDamageEffect);

                    // Give Damages
                    damageEffect.PhysicalDamage = _stompDamage;
                    damageEffect.PoiseDamage = _stompDamage;

                    // Process The Damage Effect
                    character.CharacterEffectsManager.ProcessInstantEffect(damageEffect);
                }
            }

        }
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
