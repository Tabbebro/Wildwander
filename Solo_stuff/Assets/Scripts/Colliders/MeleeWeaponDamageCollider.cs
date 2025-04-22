using UnityEngine;

public class MeleeWeaponDamageCollider : DamageCollider
{
    [Header("Attacking Character")]
    public CharacterManager CharacterCausingDamage;

    [Header("Weapon Attack Modifiers")]
    public float LightAttack01Modifier;
    public float LightAttack02Modifier;

    public float HeavyAttack01Modifier;
    public float HeavyAttack02Modifier;

    public float HeavyAttackHold01Modifier;
    public float HeavyAttackHold02Modifier;

    protected override void Awake() {
        base.Awake();


        if (_damageCollider == null) {
            _damageCollider.GetComponent<Collider>();
        }
        _damageCollider.enabled = false;
    }

    protected override void OnTriggerEnter(Collider other) {

        CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

        if (damageTarget == null) { return; }

        // Checks For Friendly Fire
        if(damageTarget == CharacterCausingDamage) { return; }

        _contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
        // TODO: Check If Blocking

        // TODO: Check I Frames

        DamageTarget(damageTarget);

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
        damageEffect.AngleHitFrom = Vector3.SignedAngle(CharacterCausingDamage.transform.forward, damageTarget.transform.forward, Vector3.up);

        // TODO: ATTACK TYPE: Add More When There Is More 
        switch (CharacterCausingDamage.CharacterCombatManager.CurrentAttackType) {
            case AttackType.LightAttack01:
                ApplyAttackDamageModifiers(LightAttack01Modifier, damageEffect);
                break;
            case AttackType.LightAttack02:
                ApplyAttackDamageModifiers(LightAttack02Modifier, damageEffect);
                break;
            case AttackType.HeavyAttack01:
                ApplyAttackDamageModifiers(HeavyAttack01Modifier, damageEffect);
                break;
            case AttackType.HeavyAttack02:
                ApplyAttackDamageModifiers(HeavyAttack02Modifier, damageEffect);
                break;
            case AttackType.HeavyAttackHold01:
                ApplyAttackDamageModifiers(HeavyAttackHold01Modifier, damageEffect);
                break;
            case AttackType.HeavyAttackHold02:
                ApplyAttackDamageModifiers(HeavyAttackHold02Modifier, damageEffect);
                break;
            default:
                break;
        }

        if (CharacterCausingDamage.IsOwner) {
            damageTarget.CharacterNetworkManager.NotifyServerOfCharacterDamageServerRpc(
                damageTarget.NetworkObjectId, 
                CharacterCausingDamage.NetworkObjectId,
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

    void ApplyAttackDamageModifiers(float modifier, TakeDamageEffect damage) {
        damage.PhysicalDamage *= modifier;
        damage.MagicDamage *= modifier;
        damage.FireDamage *= modifier;
        damage.LightningDamage *= modifier;
        damage.HolyDamage *= modifier;
    }
}
