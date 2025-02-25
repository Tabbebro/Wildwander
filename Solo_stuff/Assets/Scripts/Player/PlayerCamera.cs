using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera Instance;
    public PlayerManager _player;
    public Camera CameraObject;
    [SerializeField] Transform _cameraPivotTransform;


    [Header("Camera Settings")]
    float _cameraSmoothSpeed = 1;
    [SerializeField] float _leftAndRightRotationSpeed = 220;
    [SerializeField] float _upAndDownRotationSpeed = 220;
    [SerializeField] float _minimumPivot = -30;
    [SerializeField] float _maximumPivot = 60;
    [SerializeField] float _cameraCollisionRadius = 0.2f;
    [SerializeField] LayerMask _collideWithLayers;

    [Header("Camera Values")]
    Vector3 _cameraVelocity;
    Vector3 _cameraObjectPosition;
    [ReadOnly][SerializeField] float _leftAndRightLookAngle;
    [ReadOnly][SerializeField] float _upAndDownLookAngle;
    float _cameraZPosition;
    float _targetCameraZPosition;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        DontDestroyOnLoad(gameObject);
        _cameraZPosition = CameraObject.transform.localPosition.z;
    }


    public void HandleAllCameraActions() {
        if (_player != null) {
            HandleFollowTarget();
            HandleRotations();
            HandleCollision();
        }
    }

    void HandleFollowTarget() {
        Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, _player.transform.position, ref _cameraVelocity, _cameraSmoothSpeed * Time.deltaTime);
        transform.position = targetCameraPosition;
    }

    void HandleRotations() {

        // Rotate Left & Right
        _leftAndRightLookAngle += (PlayerInputManager.Instance._cameraHorizontalInput * _leftAndRightRotationSpeed) * Time.deltaTime;

        // Rotate Up & Down
        _upAndDownLookAngle -= (PlayerInputManager.Instance._cameraVerticalInput * _upAndDownRotationSpeed) * Time.deltaTime;
        _upAndDownLookAngle = Mathf.Clamp(_upAndDownLookAngle, _minimumPivot, _maximumPivot);

        Vector3 cameraRotation = Vector3.zero;
        Quaternion targetRotation;

        // Left & Right
        cameraRotation.y = _leftAndRightLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        transform.rotation = targetRotation;

        // Up & Down
        cameraRotation = Vector3.zero;
        cameraRotation.x = _upAndDownLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        _cameraPivotTransform.localRotation = targetRotation;

    }

    void HandleCollision() {
        _targetCameraZPosition = _cameraZPosition;
        RaycastHit hit;
        Vector3 direction = CameraObject.transform.position - _cameraPivotTransform.position;
        direction.Normalize();

        // Check if object is in front of camera
        if(Physics.SphereCast(_cameraPivotTransform.position, _cameraCollisionRadius, direction, out hit, Mathf.Abs(_targetCameraZPosition), _collideWithLayers)) {
            float distanceFromHitObject = Vector3.Distance(_cameraPivotTransform.position, hit.point);
            _targetCameraZPosition = -(distanceFromHitObject - _cameraCollisionRadius);
        }

        // If target position less than collision radius, subtract collision radius
        if (Mathf.Abs(_targetCameraZPosition) < _cameraCollisionRadius) {
            _targetCameraZPosition = -_cameraCollisionRadius;
        }

        // apply final position using a lerp
        _cameraObjectPosition.z = Mathf.Lerp(CameraObject.transform.localPosition.z, _targetCameraZPosition, 0.2f);
        CameraObject.transform.localPosition = _cameraObjectPosition;
    }
}
