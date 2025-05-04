using System.Collections.Generic;
using UnityEngine;

public class KurtStompCollider : DamageCollider
{
    AIKurtCharacterManager _kurtManager;

    protected override void Awake() {
        base.Awake();

        _kurtManager = GetComponentInParent<AIKurtCharacterManager>();
    }

    public void StompAttack() {
        _kurtManager.CharacterSFXManager.PlaySoundFX(WorldSFXManager.Instance.ChooseRandomSFXFromArray(_kurtManager.KurtSoundManager.FootImpact), 1, false);
        //GameObject stompVFX = Instantiate(_kurtManager.KurtCombatManager.ImpactVFX, new Vector3(transform.position.x, -1f, transform.position.z), Quaternion.identity);
        _kurtManager.KurtEffectsManager.PlayStompVFX(new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z));

        Collider[] colliders = Physics.OverlapSphere(transform.position, _kurtManager.KurtCombatManager.StompRadius, WorldUtilityManager.Instance.GetCharacterLayers());
        List<CharacterManager> _charactersDamaged = new();

        foreach (Collider collider in colliders) {
            CharacterManager character = collider.GetComponentInParent<CharacterManager>();

            if (character != null) {

                if (_charactersDamaged.Contains(character)) { continue; }

                if (character == _kurtManager) { return; }

                _charactersDamaged.Add(character);

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
}
