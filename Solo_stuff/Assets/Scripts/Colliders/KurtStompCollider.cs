using System.Collections.Generic;
using UnityEngine;

public class KurtStompCollider : DamageCollider
{
    AIKurtCharacterManager _kurtManager;

    protected override void Awake() {
        base.Awake();
        _damageCollider = GetComponent<Collider>();
        _kurtManager = GetComponentInParent<AIKurtCharacterManager>();
    }

    protected override void DamageTarget(CharacterManager damageTarget) {

        // If target already on list return
        if (_charactersDamaged.Contains(damageTarget)) { return; }
        _charactersDamaged.Add(damageTarget);

        TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Instance.TakeDamageEffect);

        // Give Damages
        damageEffect.PhysicalDamage = PhysicalDamage;
        damageEffect.MagicDamage = MagicDamage;
        damageEffect.FireDamage = FireDamage;
        damageEffect.LightningDamage = LightningDamage;
        damageEffect.HolyDamage = HolyDamage;

        // Give Contact Point & Angle
        damageEffect.ContactPoint = _contactPoint;
        damageEffect.AngleHitFrom = Vector3.SignedAngle(_kurtManager.transform.forward, damageTarget.transform.forward, Vector3.up);

        if (_kurtManager.IsOwner) {
            damageTarget.CharacterNetworkManager.NotifyServerOfCharacterDamageServerRpc(
                damageTarget.NetworkObjectId,
                _kurtManager.NetworkObjectId,
                damageEffect.PhysicalDamage,
                damageEffect.MagicDamage,
                damageEffect.FireDamage,
                damageEffect.LightningDamage,
                damageEffect.HolyDamage,
                damageEffect.PoiseDamage,
                damageEffect.AngleHitFrom,
                damageEffect.ContactPoint.x,
                damageEffect.ContactPoint.y,
                damageEffect.ContactPoint.z);
        }

    }

    public void PlayVFX() {
        _kurtManager.KurtEffectsManager.PlayStompVFX(transform.position);
    }
    public void PlaySFX() {
        _kurtManager.CharacterSFXManager.PlaySoundFX(WorldSFXManager.Instance.ChooseRandomSFXFromArray(_kurtManager.KurtSoundManager.FootImpact), 1, false);
    }

    /* This Broke So Made It Use Actual Colliders
    public void StompAttack() {
        _kurtManager.CharacterSFXManager.PlaySoundFX(WorldSFXManager.Instance.ChooseRandomSFXFromArray(_kurtManager.KurtSoundManager.FootImpact), 1, false);
        _kurtManager.KurtEffectsManager.PlayStompVFX(new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z));

        Collider[] colliders = Physics.OverlapSphere(transform.position, _kurtManager.KurtCombatManager.StompRadius, WorldUtilityManager.Instance.GetCharacterLayers());
        List<CharacterManager> charactersDamaged = new();
        foreach (Collider collider in colliders) {
            CharacterManager character = collider.GetComponentInParent<CharacterManager>();

            if (character != null) {
                if (charactersDamaged.Contains(character)) { continue; }

                if (character == _kurtManager) { return; }

                charactersDamaged.Add(character);

                if (character.IsOwner) {
                    // TODO: Check For Block

                    TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Instance.TakeDamageEffect);

                    // Give Damages
                    damageEffect.PhysicalDamage = _kurtManager.KurtCombatManager.StompDamage;
                    damageEffect.PoiseDamage = _kurtManager.KurtCombatManager.StompDamage;

                    // Process The Damage Effect
                    character.CharacterEffectsManager.ProcessInstantEffect(damageEffect);
                }
            }
        }
    }
    */
}
