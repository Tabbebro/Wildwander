using UnityEngine;
using Unity.Netcode;

public class CharacterManager : NetworkBehaviour
{
    [Header("Status")]
    public NetworkVariable<bool> IsDead = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [HideInInspector] public CharacterController CharacterController;
    [HideInInspector] public Animator Animator;

    [HideInInspector] public CharacterNetworkManager CharacterNetworkManager;
    [HideInInspector] public CharacterEffectsManager CharacterEffectsManager;

    [Header("Flags (Character Manager)")]
    public bool IsPerformingAction = false;
    public bool IsGrounded = true;
    public bool IsJumping = false;
    public bool ApplyRootMotion = false;
    public bool CanRotate = true;
    public bool CanMove = true;


    protected virtual void Awake() {
        DontDestroyOnLoad(this);

        CharacterController = GetComponent<CharacterController>();
        CharacterNetworkManager = GetComponent<CharacterNetworkManager>();
        CharacterEffectsManager = GetComponent<CharacterEffectsManager>();
        Animator = GetComponent<Animator>();
    }

    protected virtual void Update() {

        Animator.SetBool("IsGrounded", IsGrounded);

        if (IsOwner) {
            // Position
            CharacterNetworkManager.NetworkPosition.Value = transform.position;
            // Rotation
            CharacterNetworkManager.NetworkRotation.Value = transform.rotation;
        }
        else {
            // Position
            transform.position = Vector3.SmoothDamp(
                transform.position, 
                CharacterNetworkManager.NetworkPosition.Value, 
                ref CharacterNetworkManager.NetworkPositionVelocity, 
                CharacterNetworkManager.NetworkPositionSmoothTime);

            // Rotation
            transform.rotation = Quaternion.Slerp(
                transform.rotation, 
                CharacterNetworkManager.NetworkRotation.Value, 
                CharacterNetworkManager.NetworkRotationSmoothTime);
        }
    }

    protected virtual void LateUpdate() {
        
    }


}
