using UnityEngine;

public class PlayerMovementManager : CharacterMovementManager
{
    PlayerManager _player;
    [ReadOnly] public float _verticalMovement;
    [ReadOnly] public float _horizontalMovement;
    [ReadOnly] public float _moveAmount;

    [Header("Movement Settings")]
    Vector3 _moveDirection;
    Vector3 _targetRotationDirection;
    [SerializeField] float _walkingSpeed = 2;
    [SerializeField] float _runningSpeed = 5;
    [SerializeField] float _sprinttingSpeed = 8;
    [SerializeField] float _rotationSpeed = 15;
    [SerializeField] int _sprintingStaminaCost = 2;

    [Header("Dodge")]
    Vector3 _rollDirection;
    [SerializeField] float _dodgeStaminaCost = 25;
    

    protected override void Awake() {
        base.Awake();

        _player = GetComponent<PlayerManager>();
    }

    protected override void Update() {
        base.Update();

        if (_player.IsOwner) {
            _player._characterNetworkManager.VerticalMovement.Value = _verticalMovement;
            _player._characterNetworkManager.HorizontalMovement.Value = _horizontalMovement;
            _player._characterNetworkManager.MoveAmount.Value = _moveAmount;
        }
        else {
            _moveAmount = _player._characterNetworkManager.MoveAmount.Value;
            _horizontalMovement = _player._characterNetworkManager.HorizontalMovement.Value;
            _verticalMovement = _player._characterNetworkManager.VerticalMovement.Value;

            _player._playerAnimatorManager.UpdateAnimatorMovementParameters(0, _moveAmount, _player.isSprinting);
        }
    }

    private void GetMovementValues() {
        _verticalMovement = PlayerInputManager.Instance.VerticalInput;
        _horizontalMovement = PlayerInputManager.Instance.HorizontalInput;
        _moveAmount = PlayerInputManager.Instance.MoveAmount;
    }

    public void HandleAllMovement() {

            HandleMovement();
            HandleRotation();
    }

    void HandleMovement() {
        // Flag check
        if (!_player.canMove) { return; }

        GetMovementValues();

        // Move direction based on cameras perspective & inputs
        _moveDirection = PlayerCamera.Instance.transform.forward * _verticalMovement;
        _moveDirection = _moveDirection + PlayerCamera.Instance.transform.right * _horizontalMovement;
        _moveDirection.Normalize();
        _moveDirection.y = 0;

        // Sprinting
        if (_player.isSprinting) {
            _player._characterController.Move(_moveDirection * _sprinttingSpeed * Time.deltaTime);
        }
        // Walking
        else {
            if (PlayerInputManager.Instance.MoveAmount > 0.5f) {
                _player._characterController.Move(_moveDirection * _runningSpeed * Time.deltaTime);
            }
            else if (PlayerInputManager.Instance.MoveAmount <= 0.5f) {

                _player._characterController.Move(_moveDirection * _walkingSpeed * Time.deltaTime);
            }
        }

    }

    void HandleRotation() {
        // Flag check
        if (!_player.canRotate) { return; }

        _targetRotationDirection = Vector3.zero;
        _targetRotationDirection = PlayerCamera.Instance.CameraObject.transform.forward * _verticalMovement;
        _targetRotationDirection = _targetRotationDirection + PlayerCamera.Instance.CameraObject.transform.right * _horizontalMovement;
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
        if(_player.isPerformingAction || _player.CurrentStamina <= 0) { return; }


        // Rolling
        if (PlayerInputManager.Instance.MoveAmount > 0) {

            _rollDirection = PlayerCamera.Instance.CameraObject.transform.forward * PlayerInputManager.Instance.VerticalInput;
            _rollDirection += PlayerCamera.Instance.CameraObject.transform.right * PlayerInputManager.Instance.HorizontalInput;

            _rollDirection.y = 0;
            _rollDirection.Normalize();
        
            Quaternion playerRotation = Quaternion.LookRotation(_rollDirection);
            _player.transform.rotation = playerRotation;

            _player._playerAnimatorManager.PlayTargetActionAnimation("Roll_Forward_01", true);
        }
        // Backstep
        else {

            _player._playerAnimatorManager.PlayTargetActionAnimation("Back_Step_01", true);
        }

        _player.SetCurrentStamina(_player.CurrentStamina - _dodgeStaminaCost);
    }

    public void HandleSprint() {
        // If performing action set to false
        if (_player.isPerformingAction) {
            _player.isSprinting = false;
        }

        // Check for stamina
        if (_player.CurrentStamina <= 0) {
            _player.isSprinting = false;
            return;
        }

        // If moving set to true
        if (_moveAmount >= 0.5f) {
            _player.isSprinting = true;
        }
        // If stationary / moving too slow set to false
        else {
            _player.isSprinting = false;
        }

        if (_player.isSprinting) {
            //_player.CurrentStamina -= _sprintingStaminaCost * Time.deltaTime;
            _player.SetCurrentStamina(_player.CurrentStamina - _sprintingStaminaCost * Time.deltaTime);
        }

    }
}
