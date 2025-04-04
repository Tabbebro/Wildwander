using UnityEngine;

public class CharacterCombatManager : MonoBehaviour
{
    [Header("Attack Target")]
    public CharacterManager CurrentTarget;
    
    [Header("Attack Type")]
    public AttackType CurrentAttackType;

    [Header("Lock On Trasform")]
    public Transform LockOnTransform;

    protected virtual void Awake() {

    }
}
