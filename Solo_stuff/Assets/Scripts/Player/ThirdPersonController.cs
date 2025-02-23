using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UIElements;

public class ThirdPersonController : MonoBehaviour
{
    CharacterController _controller;
    public Inputs Inputs;

    [Header("Player Movement")]
    // Show Variables
    [SerializeField] Transform _cameraTransform;
    [SerializeField] float _walkSpeed = 5.0f;
    [SerializeField] float _runSpeed = 8.0f;
    [SerializeField] float _dodgeSpeed = 10.0f;
    [SerializeField] float _accelDuration = 1.0f;
    [SerializeField] float _decelDuration = 1.0f;
    [SerializeField] float _turnSmoothTime = 0.1f;

    // ReadOnly Variables
    [ReadOnly][SerializeField] float _speed;
    [ReadOnly][SerializeField] bool _isRunning = false;
    [ReadOnly][SerializeField] bool _isMoving = false;
    [ReadOnly][SerializeField] bool _isDodgin = false;

    [ReadOnly][SerializeField] Vector3 _moveDirection; 
    [ReadOnly][SerializeField] float _targetAngle;
    
    // Private Variables
    Vector2 _moveInput;
    Vector3 _direction;
    float _angle;
    float _turnSmoothVelocity;

    [Header("Animation Variables")]
    [SerializeField] Animator _animator;


    private void Start() {
        _controller = GetComponent<CharacterController>();
    }

    #region On-Stuff
    private void OnEnable() {
        Inputs = new();
        Inputs.Player.Enable();
        _speed = _walkSpeed;

        Inputs.Player.Run.performed += Run;

        Inputs.Player.Dodge.performed += StartDodge;
    }

    private void OnDisable() {

        Inputs.Player.Run.performed -= Run;

        Inputs.Player.Dodge.performed -= StartDodge;

        Inputs.Player.Disable();
    }

    private void OnDestroy() {

        Inputs.Dispose();
    }
    #endregion

    private void Update() {
        Movement();
        CheckDodgeSpeed();
        Animations();
    }

    void Movement() {

        if (!Inputs.Player.Move.IsPressed()) { _isMoving = false;  return;}

        _isMoving = true;


        _moveInput = Inputs.Player.Move.ReadValue<Vector2>();

        _direction = new Vector3(_moveInput.x, 0.0f, _moveInput.y).normalized;

        _targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y;

        _angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);

        transform.rotation = Quaternion.Euler(0.0f, _angle, 0.0f);

        _moveDirection = Quaternion.Euler(0f, _targetAngle, 0f) * Vector3.forward;

        _controller.Move(_moveDirection.normalized * _speed * Time.deltaTime);
    }

    void Run(InputAction.CallbackContext context) {
        if (!_isRunning && _isMoving) {
            _isRunning = true;
            StartCoroutine(Running());
        }
    }

    IEnumerator Running() {
        // Start Running and acceleration
        while (Inputs.Player.Run.IsPressed()) {
            if (_speed != _runSpeed) {
                for (float t = 0f; t < _accelDuration; t += Time.deltaTime) {
                    _speed = Mathf.Lerp(_walkSpeed, _runSpeed, t / _accelDuration);
                    yield return null;
                }
                _speed = _runSpeed;
            }
            yield return null;
        }
        // Decelerate
        _isRunning = false;
        for (float t = 0f; t < _decelDuration; t += Time.deltaTime) {
            _speed = Mathf.Lerp(_runSpeed, _walkSpeed, t / _decelDuration);
            yield return null;
        }
        _speed = _walkSpeed;
    }

    void StartDodge(InputAction.CallbackContext context) {
        _animator.SetTrigger("Dodge");
    }

    public void SetDodgeBool(bool value) {
        _isDodgin = value;
    }

    void CheckDodgeSpeed() {
        if (!_isDodgin) {
            if (_speed == _dodgeSpeed) {
                _speed = _walkSpeed;
            }
            return; 
        }

        _speed = _dodgeSpeed;
    }


    void Animations() {

        // Player Movement Animation
        if (_isMoving) {
            _animator.SetBool("IsMoving", true);

        }
        else {
            _animator.SetBool("IsMoving", false);
        }

        // Player Running animation
        if (_isRunning) {
            _animator.SetBool("IsRunning", true);
        }
        else {
            _animator.SetBool("IsRunning", false);
        }
    }

}
