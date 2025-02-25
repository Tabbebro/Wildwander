using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance;

    Inputs _inputs;

    [Header("Player Movement Input")]
    [ReadOnly][SerializeField] Vector2 _movementInput;
    [ReadOnly] public float _horizontalInput;
    [ReadOnly] public float _verticalInput;
    [ReadOnly] public float _moveAmount;


    [Header("Camera Movement Input")]
    [ReadOnly][SerializeField] Vector2 _cameraInput;
    [ReadOnly] public float _cameraHorizontalInput;
    [ReadOnly] public float _cameraVerticalInput;

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

        SceneManager.activeSceneChanged += OnSceneChange;
        
        Instance.enabled = false;
    }

    private void OnSceneChange(Scene oldScene, Scene newScene) {
        if (newScene.buildIndex == WorldSaveGameManager.Instance.GetWorldSceneIndex()) {
            Instance.enabled = true;
        }
        else {
            Instance.enabled = false;
        }
    }

    private void OnEnable() {
        if (_inputs == null) {
            _inputs = new();

            _inputs.PlayerMovement.Movement.performed += i => _movementInput = i.ReadValue<Vector2>();
            _inputs.PlayerCamera.Movement.performed += i => _cameraInput = i.ReadValue<Vector2>();
        }

        _inputs.PlayerMovement.Enable();
        _inputs.PlayerCamera.Enable();
    }

    private void OnDestroy() {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    private void OnApplicationFocus(bool focus) {
        if (enabled) {
            if (focus) {
                _inputs.PlayerMovement.Enable();
                _inputs.PlayerCamera.Enable();
            }
            else {
                _inputs.PlayerMovement.Disable();
                _inputs.PlayerCamera.Disable();
            }
        }
    }

    private void Update() {
        HandlePlayerMovementInput();
        HandleCameraMovementInput();
    }

    void HandlePlayerMovementInput() {

        _horizontalInput = _movementInput.x;
        _verticalInput = _movementInput.y;

        _moveAmount = Mathf.Clamp01(Mathf.Abs(_horizontalInput) + Mathf.Abs(_verticalInput));

        // clamp movement
        if (_moveAmount <= 0.5f && _moveAmount > 0) {
            _moveAmount = 0.5f;
        }
        else if (_moveAmount > 0.5f && _moveAmount <= 1) {
            _moveAmount = 1;
        }
    }

    void HandleCameraMovementInput() {
        _cameraHorizontalInput = _cameraInput.x;
        _cameraVerticalInput = _cameraInput.y;
    }
}
