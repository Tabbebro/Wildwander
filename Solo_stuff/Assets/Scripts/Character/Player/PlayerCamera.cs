using UnityEngine;
using System.Collections.Generic;
using System.Collections;

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
    [SerializeField] float _lockOnTargetFollowSpeed = 0.2f;
    [SerializeField] float _setCameraHeightSpeed = 0.05f;
    [SerializeField] float _unlockedCameraHeight = 1.5f;
    [SerializeField] float _lockedCameraHeight = 2.0f;
    Coroutine _cameraLockOnHeightCoroutine;
    List<CharacterManager> _availableTargets = new();
    public CharacterManager NearestLockOnTarget;
    public CharacterManager LeftLockOnTarget;
    public CharacterManager RightLockOnTarget;

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
        // Locked On
        if (Player.PlayerNetworkManager.IsLockedOn.Value) {

            // UP/DOWN Rotation
            Vector3 rotationDirection = Player.PlayerCombatManager.CurrentTarget.CharacterCombatManager.LockOnTransform.position - transform.position;
            rotationDirection.Normalize();
            rotationDirection.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _lockOnTargetFollowSpeed);

            // Left/Right Rotation
            rotationDirection = Player.PlayerCombatManager.CurrentTarget.CharacterCombatManager.LockOnTransform.position - _cameraPivotTransform.position;
            rotationDirection.Normalize();

            targetRotation = Quaternion.LookRotation(rotationDirection);
            _cameraPivotTransform.transform.rotation = Quaternion.Slerp(_cameraPivotTransform.rotation, targetRotation, _lockOnTargetFollowSpeed);

            // Save Rotation Values To Prevent Snapping When Unlocking Camera
            _upAndDownLookAngle = transform.eulerAngles.x;
            _leftAndRightLookAngle = transform.eulerAngles.y;
        }
        // Not Locked On
        else {

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
        float shortestDistanceOfLeftTarget = -Mathf.Infinity;
        float shortestDistanceOfRightTarget = Mathf.Infinity;

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

                // If Target Outside FOV / Blocked By Enviro Check Next Target
                if (viewableAngle > _minimumViewableAngle && viewableAngle < _maximumViewableAngle) {
                    RaycastHit hit;

                    // TODO: Add LayerMask For Environment Layers Only
                    if (Physics.Linecast(Player.PlayerCombatManager.LockOnTransform.position, lockOnTarget.CharacterCombatManager.LockOnTransform.position, out hit, WorldUtilityManager.Instance.GetEnviroLayers())) {
                        // Hit Environment
                        continue;
                    }
                    else {
                        _availableTargets.Add(lockOnTarget);
                    }
                }
            }
        }

        // Check Closest & Lock On To That
        for (int i = 0; i < _availableTargets.Count; i++) {
            if (_availableTargets[i] != null) {
                float distanceFromTarget = Vector3.Distance(Player.transform.position, _availableTargets[i].transform.position);

                if (distanceFromTarget < shortestDistance) {
                    shortestDistance = distanceFromTarget;
                    NearestLockOnTarget = _availableTargets[i];
                }

                // If Locked On Search For Left & Right Targets
                if (Player.PlayerNetworkManager.IsLockedOn.Value) {
                    Vector3 relativeEnemyPosition = Player.transform.InverseTransformPoint(_availableTargets[i].transform.position);
                    var distanceFromLeftTarget = relativeEnemyPosition.x;
                    var distanceFromRightTarget = relativeEnemyPosition.x;

                    // Checks For Player
                    if (_availableTargets[i] == Player.PlayerCombatManager.CurrentTarget) { continue; }


                    // Checks Left Targets
                    if (relativeEnemyPosition.x <= 0.00f && distanceFromLeftTarget > shortestDistanceOfLeftTarget) {
                        shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                        LeftLockOnTarget = _availableTargets[i];
                    }
                    // Checks Right Targets
                    else if (relativeEnemyPosition.x >= 0.00f && distanceFromRightTarget < shortestDistanceOfRightTarget) { 
                        shortestDistanceOfRightTarget = distanceFromRightTarget;
                        RightLockOnTarget = _availableTargets[i];
                    }
                }
            }
            else {
                ClearLockOnTargets();
                Player.PlayerNetworkManager.IsLockedOn.Value = false;
            }
        }

    }

    public void SetLockCameraHeight() {
        if (_cameraLockOnHeightCoroutine != null) {
            StopCoroutine(_cameraLockOnHeightCoroutine);
        }

        _cameraLockOnHeightCoroutine = StartCoroutine(SetCameraHeight());
    }

    public void ClearLockOnTargets() {
        NearestLockOnTarget = null;
        LeftLockOnTarget = null;
        RightLockOnTarget = null;
        _availableTargets.Clear();
    }

    public IEnumerator WaitThenFindNewTarget() {

        while (Player.IsPerformingAction) {
            yield return null;
        }

        ClearLockOnTargets();
        HandleLocatingLockOnTargets();

        if(NearestLockOnTarget != null) {
            Player.CharacterCombatManager.SetTarget(NearestLockOnTarget);
            Player.PlayerNetworkManager.IsLockedOn.Value = true;
        }

        yield return null;
    }

    public IEnumerator SetCameraHeight() {
        float duration = 2;
        float timer = 0;
        Vector3 velocity = Vector3.zero;
        Vector3 newLockedCameraHeight = new Vector3(_cameraPivotTransform.transform.localPosition.x, _lockedCameraHeight);
        Vector3 newUnlockedCameraHeight = new Vector3(_cameraPivotTransform.transform.localPosition.x, _unlockedCameraHeight);

        while(timer < duration) {
            timer += Time.deltaTime;
            if (Player != null) {
                if (Player.PlayerCombatManager.CurrentTarget != null) {
                    _cameraPivotTransform.transform.localPosition = 
                        Vector3.SmoothDamp(_cameraPivotTransform.transform.localPosition, newLockedCameraHeight, ref velocity, _setCameraHeightSpeed);
                    _cameraPivotTransform.transform.localRotation = 
                        Quaternion.Slerp(_cameraPivotTransform.transform.localRotation, Quaternion.Euler(0, 0, 0), _lockOnTargetFollowSpeed);
                }
                else {
                    _cameraPivotTransform.transform.localPosition = 
                        Vector3.SmoothDamp(_cameraPivotTransform.transform.localPosition, newUnlockedCameraHeight, ref velocity, _setCameraHeightSpeed);
                }
            }
            yield return null;
        }
        
        if (Player != null) {
            if (Player.PlayerCombatManager.CurrentTarget != null) {
                _cameraPivotTransform.transform.localPosition = newLockedCameraHeight;
                _cameraPivotTransform.transform.localRotation = Quaternion.Euler(0,0,0);
            }
            else {
                _cameraPivotTransform.transform.localPosition = newUnlockedCameraHeight;
            }
        }
        
        yield return null;
    }
}
