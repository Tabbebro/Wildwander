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
    [SerializeField] float _rotationSpeed = 15;

    [Header("Dodge")]
    Vector3 _rollDirection;
    

    protected override void Awake() {
        base.Awake();

        _player = GetComponent<PlayerManager>();
    }

    private void GetVerticalAndHorizontalInputs() {
        _verticalMovement = PlayerInputManager.Instance.VerticalInput;
        _horizontalMovement = PlayerInputManager.Instance.HorizontalInput;
    }

    public void HandleAllMovement() {

            HandleMovement();
            HandleRotation();
    }

    void HandleMovement() {
        // Flag check
        if (!_player.canMove) { return; }

        GetVerticalAndHorizontalInputs();

        // Move direction based on cameras perspective & inputs
        _moveDirection = PlayerCamera.Instance.transform.forward * _verticalMovement;
        _moveDirection = _moveDirection + PlayerCamera.Instance.transform.right * _horizontalMovement;
        _moveDirection.Normalize();
        _moveDirection.y = 0;

        if (PlayerInputManager.Instance.MoveAmount > 0.5f) {
            _player._characterController.Move(_moveDirection * _runningSpeed * Time.deltaTime);
        }
        else if (PlayerInputManager.Instance.MoveAmount <= 0.5f) {

            _player._characterController.Move(_moveDirection * _walkingSpeed * Time.deltaTime);
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

        if(_player.isPerformingAction) { return; }

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
    }

    public float GetSpeed() {
        if (PlayerInputManager.Instance.MoveAmount > 0.5f) {
            return _runningSpeed;
        }
        else if (PlayerInputManager.Instance.MoveAmount <= 0.5f) {

            return _walkingSpeed;
        }
        else {
            return 0;
        }
    }
}
