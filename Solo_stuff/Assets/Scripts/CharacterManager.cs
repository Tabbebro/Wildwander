using UnityEngine;
using Unity.Netcode;

public class CharacterManager : NetworkBehaviour
{
    [HideInInspector] public CharacterController _characterController;
    [HideInInspector] public Animator _animator;

    [HideInInspector] public CharacterNetworkManager _characterNetworkManager;

    [Header("Is Player?")]
    public bool IsPlayer = false;

    [Header("Flags (Character Manager)")]
    public bool isPerformingAction = false;
    public bool applyRootMotion = false;
    public bool canRotate = true;
    public bool canMove = true;

    [Header("Stats")]
    public int Endurance = 1;
    public float CurrentStamina { get; private set; }
    public int MaxStamina = 0;


    protected virtual void Awake() {
        DontDestroyOnLoad(this);

        _characterController = GetComponent<CharacterController>();
        _characterNetworkManager = GetComponent<CharacterNetworkManager>();
        _animator = GetComponent<Animator>();
    }

    protected virtual void Update() {

        if (IsOwner) {
            // Position
            _characterNetworkManager.NetworkPosition.Value = transform.position;
            // Rotation
            _characterNetworkManager.NetworkRotation.Value = transform.rotation;
        }
        else {
            // Position
            transform.position = Vector3.SmoothDamp(
                transform.position, 
                _characterNetworkManager.NetworkPosition.Value, 
                ref _characterNetworkManager.NetworkPositionVelocity, 
                _characterNetworkManager.NetworkPositionSmoothTime);

            // Rotation
            transform.rotation = Quaternion.Slerp(
                transform.rotation, 
                _characterNetworkManager.NetworkRotation.Value, 
                _characterNetworkManager.NetworkRotationSmoothTime);
        }
    }

    protected virtual void LateUpdate() {
        
    }


    public virtual void SetCurrentStamina(float newStamina) {
        float OldStamina = CurrentStamina;
        if (IsPlayer) {
            PlayerUIManager.Instance.PlayerUIHudManager.SetNewStaminaValue(OldStamina, newStamina);
        }
        CurrentStamina = newStamina;
    }

}
