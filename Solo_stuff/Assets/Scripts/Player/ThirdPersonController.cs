using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    CharacterController _controller;
    public Inputs Inputs;

    [Header("Player Movement")]
    // Show Variables
    [SerializeField] Transform _cameraTransform;
    [SerializeField] float _speed = 1.0f;
    [SerializeField] float _turnSmoothTime = 0.1f;

    // ReadOnly Variables
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
        Inputs = new();
        Inputs.Enable();
    }

    private void Update() {
        if (Inputs.Player.Move.IsPressed()) {
            Movement();
        }
    }

    private void Movement() {

        _moveInput = Inputs.Player.Move.ReadValue<Vector2>();

        _direction = new Vector3(_moveInput.x, 0.0f, _moveInput.y).normalized;

        _targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y;

        _angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);

        transform.rotation = Quaternion.Euler(0.0f, _angle, 0.0f);

        _moveDirection = Quaternion.Euler(0f, _targetAngle, 0f) * Vector3.forward;

        _controller.Move(_moveDirection.normalized * _speed * Time.deltaTime);
    }

    #region On-Stuff
    private void OnEnable() {

        if(Inputs != null) {
            Inputs.Player.Enable();
        }
    }

    private void OnDisable() {

        Inputs.Player.Disable();
    }

    private void OnDestroy() {

        Inputs.Dispose();
    }
    #endregion
}
