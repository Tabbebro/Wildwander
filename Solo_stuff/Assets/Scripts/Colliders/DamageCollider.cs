using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [Header("Collider")]
    [SerializeField] protected Collider _damageCollider;

    [Header("Damage")]
    public float PhysicalDamage = 0; // TODO: Split Into Slashing, Piercing & Bludgeoning
    public float MagicDamage = 0;
    public float FireDamage = 0;
    public float LightningDamage = 0;
    public float HolyDamage = 0;
    // TODO: Make More Damage Types

    [Header("Contact Point")]
    protected Vector3 _contactPoint;

    [Header("Characters Damaged")]
    protected List<CharacterManager> _charactersDamaged = new();

    [Header("Block")]
    protected Vector3 _directionFromAttackToDamageTarget;
    protected float _dotValueFromAttackToDamageTarget;

    protected virtual void Awake() {
        
    }

    protected virtual void OnTriggerEnter(Collider other) {
        CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

        if(damageTarget == null) { return; }

        _contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

        // TODO: Check For Friendly Fire

        // TODO: Check If Blocking
        CheckForBlock(damageTarget);

        DamageTarget(damageTarget);
    }

    protected virtual void CheckForBlock(CharacterManager damageTarget) {
        if (_charactersDamaged.Contains(damageTarget)) { return; }

        GetBlockedDotValues(damageTarget);

        // TODO: Check If This Dot Product Value Is Good Or Not
        if (damageTarget.CharacterNetworkManager.IsBlocking.Value && _dotValueFromAttackToDamageTarget > 0.3f) {
            _charactersDamaged.Add(damageTarget);

            TakeBlockedDamageEffect blockedDamageEffect = Instantiate(WorldCharacterEffectsManager.Instance.TakeBlockedDamageEffect);

            // Give Damages
            blockedDamageEffect.PhysicalDamage = PhysicalDamage;
            blockedDamageEffect.MagicDamage = MagicDamage;
            blockedDamageEffect.FireDamage = FireDamage;
            blockedDamageEffect.LightningDamage = LightningDamage;
            blockedDamageEffect.HolyDamage = HolyDamage;

            // Give Contact Point
            blockedDamageEffect.ContactPoint = _contactPoint;

            // Process The Blocked Damage Effect
            damageTarget.CharacterEffectsManager.ProcessInstantEffect(blockedDamageEffect);
        }
    }

    protected virtual void GetBlockedDotValues(CharacterManager damageTarget) {
        _directionFromAttackToDamageTarget = transform.position - damageTarget.transform.position;
        _dotValueFromAttackToDamageTarget = Vector3.Dot(_directionFromAttackToDamageTarget, damageTarget.transform.forward);
    }

    protected virtual void DamageTarget(CharacterManager damageTarget) {
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

        // Give Contact Point
        damageEffect.ContactPoint = _contactPoint;

        // Process The Damage Effect
        damageTarget.CharacterEffectsManager.ProcessInstantEffect(damageEffect);
    }

    public virtual void EnableDamageCollider() {
        _damageCollider.enabled = true;
    }

    public virtual void DisableDamageCollider() { 
        _damageCollider.enabled = false;
        _charactersDamaged.Clear();
    }

}
