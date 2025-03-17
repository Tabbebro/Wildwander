using UnityEngine;
using Unity.Netcode;

public class CharacterNetworkManager : NetworkBehaviour
{
    CharacterManager _character;
    [Header("Position")]
    public NetworkVariable<Vector3> NetworkPosition = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<Quaternion> NetworkRotation = new NetworkVariable<Quaternion>(Quaternion.identity, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public Vector3 NetworkPositionVelocity;
    public float NetworkPositionSmoothTime = 0.1f;
    public float NetworkRotationSmoothTime = 0.1f;

    [Header("Animator")]
    public NetworkVariable<float> HorizontalMovement = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> VerticalMovement = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> MoveAmount = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Flags")]
    public NetworkVariable<bool> IsSprinting = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Stats")]
    public NetworkVariable<int> Vitality = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> Endurance = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Resources")]
    // Health Atributes Comes From Vitality
    public NetworkVariable<float> CurrentHealth = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> MaxHealth = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    // Stamina Atributes Comes From Endurance
    public NetworkVariable<float> CurrentStamina = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> MaxStamina = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    protected virtual void Awake() {
        _character = GetComponent<CharacterManager>();
    }

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

}
