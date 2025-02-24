using UnityEngine;

public class PlayerMovementManager : CharacterMovementManager
{
    PlayerManager _player;
    [ReadOnly] public float _verticalMovement;
    [ReadOnly] public float _horizontalMovement;
    [ReadOnly] public float _moveAmount;

    Vector3 _moveDirection;
    Vector3 _targetRotationDirection;
    [SerializeField] float _walkingSpeed = 2;
    [SerializeField] float _runningSpeed = 5;
    [SerializeField] float _rotationSpeed = 15;
    

    protected override void Awake() {
        base.Awake();

        _player = GetComponent<PlayerManager>();
    }

    private void GetVerticalAndHorizontalInputs() {
        _verticalMovement = PlayerInputManager.Instance._verticalInput;
        _horizontalMovement = PlayerInputManager.Instance._horizontalInput;
    }

    public void HandleAllMovement() {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement() {

        GetVerticalAndHorizontalInputs();

        // Move direction based on cameras perspective & inputs
        _moveDirection = PlayerCamera.Instance.transform.forward * _verticalMovement;
        _moveDirection = _moveDirection + PlayerCamera.Instance.transform.right * _horizontalMovement;
        _moveDirection.Normalize();
        _moveDirection.y = 0;

        if (PlayerInputManager.Instance._moveAmount > 0.5f) {

            _player._characterController.Move(_moveDirection * _runningSpeed * Time.deltaTime);
        }
        else if (PlayerInputManager.Instance._moveAmount <= 0.5f) {

            _player._characterController.Move(_moveDirection * _walkingSpeed * Time.deltaTime);

        }
    }

    void HandleRotation() {

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
}
