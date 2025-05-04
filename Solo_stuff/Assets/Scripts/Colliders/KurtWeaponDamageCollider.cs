using UnityEngine;

public class KurtWeaponDamageCollider : DamageCollider
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
}   
