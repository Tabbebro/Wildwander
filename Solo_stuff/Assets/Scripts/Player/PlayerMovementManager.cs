using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovementManager : CharacterMovementManager
{
    PlayerManager _player;
    [ReadOnly] public float VerticalMovement;
    [ReadOnly] public float HorizontalMovement;
    [ReadOnly] public float MoveAmount;

    [Header("Movement Settings")]
    [SerializeField] float _walkingSpeed = 2;
    [SerializeField] float _runningSpeed = 5;
    [SerializeField] float _sprinttingSpeed = 8;
    [SerializeField] float _rotationSpeed = 15;
    [SerializeField] int _sprintingStaminaCost = 2;
    Vector3 _moveDirection;
    Vector3 _targetRotationDirection;

    [Header("Jump")]
    [SerializeField] float _jumpHeight = 4;
    [SerializeField] float _jumpForwardSpeed = 5;
    [SerializeField] float _freeFallSpeed = 5;
    [SerializeField] float _jumpStaminaCost = 25;
    Vector3 _jumpDirection;

    [Header("Dodge")]
    [SerializeField] float _dodgeStaminaCost = 25;
    Vector3 _rollDirection;
    

    protected override void Awake() {
        base.Awake();

        _player = GetComponent<PlayerManager>();
    }

    protected override void Update() {
        base.Update();

        if (_player.IsOwner) {
            _player.CharacterNetworkManager.VerticalMovement.Value = VerticalMovement;
            _player.CharacterNetworkManager.HorizontalMovement.Value = HorizontalMovement;
            _player.CharacterNetworkManager.MoveAmount.Value = MoveAmount;
        }
        else {
            MoveAmount = _player.CharacterNetworkManager.MoveAmount.Value;
            HorizontalMovement = _player.CharacterNetworkManager.HorizontalMovement.Value;
            VerticalMovement = _player.CharacterNetworkManager.VerticalMovement.Value;

            _player.PlayerAnimatorManager.UpdateAnimatorMovementParameters(0, MoveAmount, _player.PlayerNetworkManager.IsSprinting.Value);
        }
    }

    private void GetMovementValues() {
        VerticalMovement = PlayerInputManager.Instance.VerticalInput;
        HorizontalMovement = PlayerInputManager.Instance.HorizontalInput;
        MoveAmount = PlayerInputManager.Instance.MoveAmount;
    }

    public void HandleAllMovement() {

        HandleGroundedMovement();
        HandleRotation();
        HandleJumpingMovement();
        HandleFreeFallMovement();
    }

    void HandleGroundedMovement() {
        // Flag check
        if (!_player.CanMove) { return; }

        GetMovementValues();

        // Move direction based on cameras perspective & inputs
        _moveDirection = PlayerCamera.Instance.transform.forward * VerticalMovement;
        _moveDirection = _moveDirection + PlayerCamera.Instance.transform.right * HorizontalMovement;
        _moveDirection.Normalize();
        _moveDirection.y = 0;

        // Sprinting
        if (_player.PlayerNetworkManager.IsSprinting.Value) {
            _player.CharacterController.Move(_moveDirection * _sprinttingSpeed * Time.deltaTime);
        }
        // Walking
        else {
            if (PlayerInputManager.Instance.MoveAmount > 0.5f) {
                _player.CharacterController.Move(_moveDirection * _runningSpeed * Time.deltaTime);
            }
            else if (PlayerInputManager.Instance.MoveAmount <= 0.5f) {

                _player.CharacterController.Move(_moveDirection * _walkingSpeed * Time.deltaTime);
            }
        }

    }

    void HandleJumpingMovement() {
        if (!_player.IsJumping) { return; }
            
        _player.CharacterController.Move(_jumpDirection * _jumpForwardSpeed * Time.deltaTime);
    }

    void HandleFreeFallMovement() {
        if (_player.IsGrounded) { return; }

        Vector3 freeFallDirection;

        freeFallDirection = PlayerCamera.Instance.CameraObject.transform.forward * PlayerInputManager.Instance.VerticalInput;
        freeFallDirection += PlayerCamera.Instance.CameraObject.transform.right * PlayerInputManager.Instance.HorizontalInput;
        freeFallDirection.y = 0;

        _player.CharacterController.Move(freeFallDirection * _freeFallSpeed * Time.deltaTime);
    }

    void HandleRotation() {
        // Flag check
        if (!_player.CanRotate) { return; }

        _targetRotationDirection = Vector3.zero;
        _targetRotationDirection = PlayerCamera.Instance.CameraObject.transform.forward * VerticalMovement;
        _targetRotationDirection = _targetRotationDirection + PlayerCamera.Instance.CameraObject.transform.right * HorizontalMovement;
        _targetRotationDirection.Normalize();
        _targetRotationDirection.y = 0;

        if (_targetRotationDirection == Vector3.zero) {
            _targetRotationDirection = transform.forward;
        }

        Quaternion newRotation = Quaternion.LookRotation(_targetRotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, _rotationSpeed * Time.deltaTime);
        transform.rotation = targetRotation;

    }

    public void AttemptToPerformDodge() {

        // Check if performing another action or if has stamina
        if(_player.IsPerformingAction || _player.PlayerNetworkManager.CurrentStamina.Value <= 0) { return; }


        // Rolling
        if (PlayerInputManager.Instance.MoveAmount > 0) {

            _rollDirection = PlayerCamera.Instance.CameraObject.transform.forward * PlayerInputManager.Instance.VerticalInput;
            _rollDirection += PlayerCamera.Instance.CameraObject.transform.right * PlayerInputManager.Instance.HorizontalInput;

            _rollDirection.y = 0;
            _rollDirection.Normalize();
        
            Quaternion playerRotation = Quaternion.LookRotation(_rollDirection);
            _player.transform.rotation = playerRotation;

            _player.PlayerAnimatorManager.PlayTargetActionAnimation("Roll_Forward_01", true);
        }
        // Backstep
        else {

            _player.PlayerAnimatorManager.PlayTargetActionAnimation("Back_Step_01", true);
        }

        // Remove Stamina
        _player.PlayerNetworkManager.CurrentStamina.Value -= _dodgeStaminaCost;
    }

    public void HandleSprint() {
        // If performing action set to false
        if (_player.IsPerformingAction) {
            _player.PlayerNetworkManager.IsSprinting.Value = false;
        }

        // Check for stamina
        if (_player.PlayerNetworkManager.CurrentStamina.Value <= 0) {
            _player.PlayerNetworkManager.IsSprinting.Value = false;
            return;
        }

        // If moving set to true
        if (MoveAmount >= 0.5f) {
            _player.PlayerNetworkManager.IsSprinting.Value = true;
        }
        // If stationary / moving too slow set to false
        else {
            _player.PlayerNetworkManager.IsSprinting.Value = false;
        }

        if (_player.PlayerNetworkManager.IsSprinting.Value) {
            _player.PlayerNetworkManager.CurrentStamina.Value -= _sprintingStaminaCost * Time.deltaTime;
        }

    }

    public void AttemptToPerformJump() {

        // Check if performing another action or if has stamina
        if (_player.IsPerformingAction || _player.PlayerNetworkManager.CurrentStamina.Value <= 0) { return; }

        // Check if jumping or not grounded
        if (_player.IsJumping || !_player.IsGrounded) { return; }

        _player.PlayerAnimatorManager.PlayTargetActionAnimation("Main_Jump_01", false);

        _player.IsJumping = true;

        _player.PlayerNetworkManager.CurrentStamina.Value -= _jumpStaminaCost;

        _jumpDirection = PlayerCamera.Instance.CameraObject.transform.forward * PlayerInputManager.Instance.VerticalInput;
        _jumpDirection += PlayerCamera.Instance.CameraObject.transform.right * PlayerInputManager.Instance.HorizontalInput;

        _jumpDirection.y = 0;

        if (_jumpDirection != Vector3.zero) {
            // Full Jump Distance If Sprinting
            if (_player.PlayerNetworkManager.IsSprinting.Value) {
                _jumpDirection *= 1;
            }
            // Half Jump Distance If Running
            else if (PlayerInputManager.Instance.MoveAmount > 0.5) {
                _jumpDirection *= 0.5f;
            }
            // Quarter Jump Distance If Walking
            else if (PlayerInputManager.Instance.MoveAmount <= 0.5) {
                _jumpDirection *= 0.25f;
            }
        }
    }

    public void ApplyJumpingVelocity() {
        _yVelocity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravityForce);
    }
}
