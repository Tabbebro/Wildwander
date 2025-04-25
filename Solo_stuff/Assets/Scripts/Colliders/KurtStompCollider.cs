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
        _kurtManager.CharacterSFXManager.PlaySoundFX(WorldSFXManager.Instance.ChooseRandomSFXFromArray(_kurtManager.KurtSoundManager.FootImpact));
        GameObject stompVFX = Instantiate(_kurtManager.KurtCombatManager.ImpactVFX, transform);
        // TODO: Do Not Hardcode
        stompVFX.transform.localPosition = new Vector3(0, -2.6f, 0);
        stompVFX.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
        stompVFX.transform.localScale = new Vector3(30, 30, 30);

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
