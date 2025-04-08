using UnityEngine;
using Unity.Netcode;
using System.Collections;
using System.Collections.Generic;

public class CharacterManager : NetworkBehaviour
{
    [Header("Status")]
    public NetworkVariable<bool> IsDead = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [HideInInspector] public CharacterController CharacterController;
    [HideInInspector] public Animator Animator;

    [HideInInspector] public CharacterNetworkManager CharacterNetworkManager;
    [HideInInspector] public CharacterMovementManager CharacterMovementManager;
    [HideInInspector] public CharacterEffectsManager CharacterEffectsManager;
    [HideInInspector] public CharacterAnimatorManager CharacterAnimatorManager;
    [HideInInspector] public CharacterCombatManager CharacterCombatManager;
    [HideInInspector] public CharacterSoundFXManager CharacterSoundFXManager;

    [Header("Flags (Character Manager)")]
    public bool IsPerformingAction = false;
    public bool IsGrounded = true;
    public bool ApplyRootMotion = false;
    public bool CanRotate = true;
    public bool CanMove = true;


    protected virtual void Awake() {
        DontDestroyOnLoad(this);

        CharacterController = GetComponent<CharacterController>();
        CharacterNetworkManager = GetComponent<CharacterNetworkManager>();
        CharacterMovementManager = GetComponent<CharacterMovementManager>();
        CharacterEffectsManager = GetComponent<CharacterEffectsManager>();
        CharacterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        CharacterCombatManager = GetComponent<CharacterCombatManager>();
        CharacterSoundFXManager = GetComponent<CharacterSoundFXManager>();
        Animator = GetComponent<Animator>();
    }

    protected virtual void Start() {
        IgnoreMyOwnColliders();
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

    public virtual IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false) {
        if (IsOwner) { 
            if (!manuallySelectDeathAnimation) {
                CharacterAnimatorManager.PlayTargetActionAnimation("Death_01", true);
            }
            CharacterNetworkManager.CurrentHealth.Value = 0;
            IsDead.Value = true;
        }

        // Play Death SFX

        yield return new WaitForSeconds(5);

        // TODO: Give Some Currency On Enemy Death

        // TODO: Disable Character
    }

    public virtual void ReviveCharacter() {

    }

    protected virtual void IgnoreMyOwnColliders() {
        Collider characterControllerCollider = GetComponent<Collider>();
        Collider[] damageableCharacterColliders = GetComponentsInChildren<Collider>();

        List<Collider> ignoreColliders = new();

        foreach (Collider c in damageableCharacterColliders) {
            ignoreColliders.Add(c);
        }

        ignoreColliders.Add(characterControllerCollider);

        // Makes Colliders Ignore Collision With Each Other. It's A Bit Spageth But Should Work
        foreach (Collider collider in ignoreColliders) {
            foreach (Collider otherCollider in ignoreColliders) {
                Physics.IgnoreCollision(collider, otherCollider, true);
            }
        }
    }
}
