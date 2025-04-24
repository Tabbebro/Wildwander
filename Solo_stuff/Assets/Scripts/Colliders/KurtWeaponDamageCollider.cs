using UnityEngine;

public class KurtWeaponDamageCollider : DamageCollider
{

    [SerializeField] AIBossCharacterManager _bossCharacter;

    protected override void Awake() {
        base.Awake();
        _damageCollider = GetComponent<Collider>();
        _bossCharacter = GetComponentInParent<AIBossCharacterManager>();
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
        damageEffect.AngleHitFrom = Vector3.SignedAngle(_bossCharacter.transform.forward, damageTarget.transform.forward, Vector3.up);

        if (_bossCharacter.IsOwner) {
            damageTarget.CharacterNetworkManager.NotifyServerOfCharacterDamageServerRpc(
                damageTarget.NetworkObjectId,
                _bossCharacter.NetworkObjectId,
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
