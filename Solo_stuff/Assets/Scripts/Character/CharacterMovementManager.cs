using UnityEngine;

public class CharacterMovementManager : MonoBehaviour
{
    CharacterManager _character;

    [Header("Ground Check & Jumping")]
    [SerializeField] protected float _gravityForce = 5.55f;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] float _groundCheckSphereRadius = 1;
    [SerializeField] protected Vector3 _yVelocity;              // Y force of the character (Jump & Fall)
    [SerializeField] protected float _groundedYVelocity = -20;  // Helps characters to stick to ground
    [SerializeField] protected float _fallStartYVelocity = 5;
    protected bool _fallingVelocityHasBeenSet = false;
    protected float _inAirTimer = 0;

    [Header("Flags")]
    public bool isRolling = false;

    protected virtual void Awake() {
        _character = GetComponent<CharacterManager>();
    }

    protected virtual void Update() {
        HandleGroundCheck();

        if (_character.IsGrounded && _yVelocity.y < 0) {
            _inAirTimer = 0;
            _fallingVelocityHasBeenSet = false;
            _yVelocity.y = _groundedYVelocity;
        }
        else {
            if (!_character.CharacterNetworkManager.IsJumping.Value && !_fallingVelocityHasBeenSet) {
                _fallingVelocityHasBeenSet = true;
                _yVelocity.y = _fallStartYVelocity;
            }
            _inAirTimer += Time.deltaTime;

            _yVelocity.y += _gravityForce * Time.deltaTime;

            _character.Animator.SetFloat("InAirTimer", _inAirTimer);

            //_character.CharacterController.Move( _yVelocity * Time.deltaTime );
        }
        _character.CharacterController.Move( _yVelocity * Time.deltaTime );
    }

    protected void HandleGroundCheck() {
        _character.IsGrounded = Physics.CheckSphere(_character.transform.position, _groundCheckSphereRadius, _groundLayer);
    }

    protected void OnDrawGizmosSelected() {
        //Gizmos.DrawSphere(_character.transform.position, _groundCheckSphereRadius);
    }
}
