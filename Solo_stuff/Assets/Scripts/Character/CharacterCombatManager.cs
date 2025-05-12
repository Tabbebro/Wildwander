using UnityEngine;
using Unity.Netcode;

public class CharacterCombatManager : NetworkBehaviour
{
    protected CharacterManager _character;

    [Header("Last Attack Animation Performed")]
    public string LastAttackAnimationPerformed;

    [Header("Attack Target")]
    public CharacterManager CurrentTarget;
    
    [Header("Attack Type")]
    public AttackType CurrentAttackType;

    [Header("Lock On Trasform")]
    public Transform LockOnTransform;

    [Header("Attack Flags")]
    public bool CanPerformRollingAttack = false;
    public bool CanPerformBackstepAttack = false;

    protected virtual void Awake() {
        _character = GetComponent<CharacterManager>();
        LockOnTransform = GetComponentInChildren<Utility_GetLockOnTarget>().transform;
    }

    public virtual void SetTarget(CharacterManager newTarget) {
        if (!_character.IsOwner) { return; }

        if (newTarget != null) {
            CurrentTarget = newTarget;
            _character.CharacterNetworkManager.CurrentTargetNetworkObjectID.Value = newTarget.GetComponent<NetworkObject>().NetworkObjectId;
        }
        else {
            CurrentTarget = null;
        }
    }

    public void EnableIsVulnerable() {
        if (_character.IsOwner) { 
            _character.CharacterNetworkManager.IsInvulnerable.Value = true;
        }
    }

    public void DisableIsVulnerable() {
        if (_character.IsOwner) {
            _character.CharacterNetworkManager.IsInvulnerable.Value = false;
        }
    }

    public void EnableCanDoRollingAttack() {
        CanPerformRollingAttack = true;
    }

    public void DisableCanDoRollingAttack() {
        CanPerformRollingAttack = false;
    }

    public void EnableCanDoBackstepAttack() {
        CanPerformBackstepAttack = true;
    }

    public void DisableCanDoBackstepAttack() {
        CanPerformBackstepAttack = false;
    }

    public virtual void EnableCanDoCombo() {

    }

    public virtual void DisableCanDoCombo() {

    }
}
