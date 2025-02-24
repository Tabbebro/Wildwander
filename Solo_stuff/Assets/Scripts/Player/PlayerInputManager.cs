using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance;

    Inputs _inputs;

    [ReadOnly][SerializeField] Vector2 _movementInput;
    [ReadOnly] public float _horizontalInput;
    [ReadOnly] public float _verticalInput;
    [ReadOnly] public float _moveAmount;

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

            _inputs.Player.Move.performed += i => _movementInput = i.ReadValue<Vector2>();
        }

        _inputs.Player.Enable();
    }

    private void OnDestroy() {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    private void OnApplicationFocus(bool focus) {
        if (enabled) {
            if (focus) {
                _inputs.Player.Enable();
            }
            else {
                _inputs.Player.Disable();
            }
        }
    }

    private void Update() {
        HandleMovementInput();
    }

    void HandleMovementInput() {

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
}
