using UnityEngine;
using Unity.Netcode;

public class CharacterNetworkManager : NetworkBehaviour {
    CharacterManager _character;
    [Header("Position")]
    public NetworkVariable<Vector3> NetworkPosition = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<Quaternion> NetworkRotation = new NetworkVariable<Quaternion>(Quaternion.identity, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public Vector3 NetworkPositionVelocity;
    public float NetworkPositionSmoothTime = 0.1f;
    public float NetworkRotationSmoothTime = 0.1f;

    [Header("Animator")]
    public NetworkVariable<bool> IsMoving = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> HorizontalMovement = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> VerticalMovement = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> MoveAmount = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Target")]
    public NetworkVariable<ulong> CurrentTargetNetworkObjectID = new NetworkVariable<ulong>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    
    [Header("Flags")]
    public NetworkVariable<bool> IsLockedOn = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> IsSprinting = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> IsJumping = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> IsChargingAttack = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Stats")]
    public NetworkVariable<int> Vitality = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> Endurance = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Resources")]
    // Health Atributes Comes From Vitality
    public NetworkVariable<int> CurrentHealth = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> MaxHealth = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    // Stamina Atributes Comes From Endurance
    public NetworkVariable<float> CurrentStamina = new NetworkVariable<float>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> MaxStamina = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    protected virtual void Awake() {
        _character = GetComponent<CharacterManager>();
    }

    public void CheckHP(int oldValue, int newValue) {


        if (CurrentHealth.Value <= 0) {
            StartCoroutine(_character.ProcessDeathEvent());
        }

        if (!_character.IsOwner) { return; }

        if (CurrentHealth.Value > MaxHealth.Value) {
            CurrentHealth.Value = MaxHealth.Value;
        }
    }

    public void OnIsChargingAttackChanged(bool oldStatus, bool newStatus) {
        _character.Animator.SetBool("IsChargingAttack", newStatus);
    }

    public void OnIsMovingChanged(bool oldValue, bool newValue) {
        _character.Animator.SetBool("IsMoving", IsMoving.Value);
    }

    #region Animation Rpc
    // BASIC ANIMATIONS //
    [ServerRpc]
    public void NotifyServerOfActionAnimationServerRpc(ulong clientID, string animationID, bool applyRootMotion) {
        if (IsServer) {
            PlayActionAnimationForAllClientsClientRpc(clientID, animationID, applyRootMotion);
        }
    }

    [ClientRpc]
    public void PlayActionAnimationForAllClientsClientRpc(ulong clientID, string animationID, bool applyRootMotion) {
        if (clientID != NetworkManager.Singleton.LocalClientId) {
            PerformActionAnimationFromServer(animationID, applyRootMotion);
        }
    }

    private void PerformActionAnimationFromServer(string animationID, bool applyRootMotion) {
        _character.ApplyRootMotion = applyRootMotion;
        _character.Animator.CrossFade(animationID, 0.2f);
    }

    // WEAPONS //

    [ServerRpc]
    public void NotifyServerOfAttackActionAnimationServerRpc(ulong clientID, string animationID, bool applyRootMotion) {
        if (IsServer) {
            PlayActionAttackAnimationForAllClientsClientRpc(clientID, animationID, applyRootMotion);
        }
    }

    [ClientRpc]
    public void PlayActionAttackAnimationForAllClientsClientRpc(ulong clientID, string animationID, bool applyRootMotion) {
        if (clientID != NetworkManager.Singleton.LocalClientId) {
            PerformAttackActionAnimationFromServer(animationID, applyRootMotion);
        }
    }

    private void PerformAttackActionAnimationFromServer(string animationID, bool applyRootMotion) {
        _character.ApplyRootMotion = applyRootMotion;
        _character.Animator.CrossFade(animationID, 0.2f);
    }
    #endregion

    #region Damage Rpc

    [ServerRpc(RequireOwnership = false)]
    public void NotifyServerOfCharacterDamageServerRpc(
        ulong damagedCharacterId,
        ulong characterCausingDamage,
        float physicalDamage,
        float magicDamage,
        float fireDamage,
        float lightningDamage,
        float holyDamage,
        float poiseDamage,
        float angleHitFrom,
        float contactPointX,
        float contactPointY,
        float contactPointZ) {

        if (IsServer) {
            NotifyServerOfCharacterDamageClientRpc(damagedCharacterId, characterCausingDamage, physicalDamage, magicDamage, fireDamage, lightningDamage, holyDamage, poiseDamage, angleHitFrom, contactPointX,contactPointY,contactPointZ);
        }

    }

    [ClientRpc]
    public void NotifyServerOfCharacterDamageClientRpc(
        ulong damagedCharacterId,
        ulong characterCausingDamage,
        float physicalDamage,
        float magicDamage,
        float fireDamage,
        float lightningDamage,
        float holyDamage,
        float poiseDamage,
        float angleHitFrom,
        float contactPointX,
        float contactPointY,
        float contactPointZ) {

        ProcessCharacterDamageFromServer(damagedCharacterId, characterCausingDamage, physicalDamage, magicDamage, fireDamage, lightningDamage, holyDamage, poiseDamage, angleHitFrom, contactPointX, contactPointY, contactPointZ);

    }

    public void ProcessCharacterDamageFromServer(
        ulong damagedCharacterID,
        ulong characterCausingDamageID,
        float physicalDamage,
        float magicDamage,
        float fireDamage,
        float lightningDamage,
        float holyDamage,
        float poiseDamage,
        float angleHitFrom,
        float contactPointX,
        float contactPointY,
        float contactPointZ) {

        CharacterManager damagedCharacter = NetworkManager.Singleton.SpawnManager.SpawnedObjects[damagedCharacterID].gameObject.GetComponent<CharacterManager>();
        CharacterManager characterCausingDamage = NetworkManager.Singleton.SpawnManager.SpawnedObjects[characterCausingDamageID].gameObject.GetComponent<CharacterManager>();

        TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Instance.TakeDamageEffect);
        damageEffect.PhysicalDamage = physicalDamage;
        damageEffect.MagicDamage = magicDamage;
        damageEffect.FireDamage = fireDamage;
        damageEffect.LightningDamage = lightningDamage;
        damageEffect.HolyDamage = holyDamage;
        damageEffect.PoiseDamage = poiseDamage;
        damageEffect.AngleHitFrom = angleHitFrom;
        damageEffect.ContactPoint = new Vector3(contactPointX, contactPointY, contactPointZ);
        damageEffect.CharacterCausingDamage = characterCausingDamage;

        damagedCharacter.CharacterEffectsManager.ProcessInstantEffect(damageEffect);
    }

    #endregion

    #region Lock On

    public void OnLockOnTargetIDChange(ulong oldID, ulong newID) {
        if (!IsOwner) {  
            _character.CharacterCombatManager.CurrentTarget = NetworkManager.Singleton.SpawnManager.SpawnedObjects[newID].gameObject.GetComponent<CharacterManager>(); 
        }
    }

    public void OnIsLockedOnChanged(bool oldValue, bool isLockedOn) {
        if (!isLockedOn) {
            _character.CharacterCombatManager.CurrentTarget = null;
        }
    }

    #endregion
}
