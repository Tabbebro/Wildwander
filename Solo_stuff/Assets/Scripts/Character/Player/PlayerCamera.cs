using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera Instance;
    public PlayerManager Player;
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

    [Header("Lock On")]
    [SerializeField] float _lockOnRadius = 20f;
    [SerializeField] float _minimumViewableAngle = -70;
    [SerializeField] float _maximumViewableAngle = 70;
    [SerializeField] float _maximumLockOnDistance = 20;

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
        if (Player != null) {
            HandleFollowTarget();
            HandleRotations();
            HandleCollision();
        }
    }

    void HandleFollowTarget() {
        Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, Player.transform.position, ref _cameraVelocity, _cameraSmoothSpeed * Time.deltaTime);
        transform.position = targetCameraPosition;
    }

    void HandleRotations() {

        // Rotate Left & Right
        _leftAndRightLookAngle += (PlayerInputManager.Instance.CameraHorizontalInput * _leftAndRightRotationSpeed) * Time.deltaTime;

        // Rotate Up & Down
        _upAndDownLookAngle -= (PlayerInputManager.Instance.CameraVerticalInput * _upAndDownRotationSpeed) * Time.deltaTime;
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

    public void HandleLocatingLockOnTargets() {
        float shortestDistance = Mathf.Infinity;
        float shortestDistanceOfLeftTarget = Mathf.Infinity;
        float shortestDistanceOfRightTarget = -Mathf.Infinity;

        // TODO: Use A LayerMask
        Collider[] colliders = Physics.OverlapSphere(Player.transform.position, _lockOnRadius, WorldUtilityManager.Instance.GetCharacterLayers());

        for (int i = 0; i < colliders.Length; i++) {
            CharacterManager lockOnTarget = colliders[i].GetComponent<CharacterManager>();

            if (lockOnTarget != null) {
                // FOV Check
                Vector3 lockOnTargetDirection = lockOnTarget.transform.position - Player.transform.position;
                float distanceFromTarget = Vector3.Distance(Player.transform.position, lockOnTarget.transform.position);
                float viewableAngle = Vector3.Angle(lockOnTargetDirection, CameraObject.transform.forward);

                // Check If Target Is Dead
                if (lockOnTarget.IsDead.Value) { continue; }

                // Check If Target Is Self
                if(lockOnTarget.transform.root == Player.transform.root) { continue; }

                // Check If Target Is Too Far
                if (distanceFromTarget > _maximumLockOnDistance) { continue; }

                if (viewableAngle > _minimumViewableAngle && viewableAngle < _maximumViewableAngle) {
                    RaycastHit hit;

                    // TODO: Add LayerMask For Environment Layers Only
                    if (Physics.Linecast(Player.PlayerCombatManager.LockOnTransform.position, lockOnTarget.CharacterCombatManager.LockOnTransform.position, out hit, WorldUtilityManager.Instance.GetEnviroLayers())) {
                        // Hit Environment
                        continue;
                    }
                    else {
                        Debug.Log("Found Lock On Target");
                    }
                }
            }
        }

    }
}
