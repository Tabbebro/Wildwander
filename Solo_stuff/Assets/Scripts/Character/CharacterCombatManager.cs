using UnityEngine;
using Unity.Netcode;

public class CharacterCombatManager : NetworkBehaviour
{
    CharacterManager _character;

    [Header("Attack Target")]
    public CharacterManager CurrentTarget;
    
    [Header("Attack Type")]
    public AttackType CurrentAttackType;

    [Header("Lock On Trasform")]
    public Transform LockOnTransform;

    protected virtual void Awake() {
        _character = GetComponent<CharacterManager>();
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
}
