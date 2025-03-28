using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance;
    public PlayerManager Player;

    Inputs _inputs;

    [Header("Player Movement Input")]
    [ReadOnly][SerializeField] Vector2 _movementInput;
    [ReadOnly] public float HorizontalInput;
    [ReadOnly] public float VerticalInput;
    [ReadOnly] public float MoveAmount;


    [Header("Camera Movement Input")]
    [ReadOnly][SerializeField] Vector2 _cameraInput;
    [ReadOnly] public float CameraHorizontalInput;
    [ReadOnly] public float CameraVerticalInput;

    [Header("Player Action Input")]
    [SerializeField] bool _dodgeInput = false;
    [SerializeField] bool _sprintInput = false;
    [SerializeField] bool _jumpInput = false;
    [SerializeField] bool _lightAttackInput = false;

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
        
        if (_inputs != null) {
            DisableInputs();
        }
    }

    private void OnSceneChange(Scene oldScene, Scene newScene) {
        if (newScene.buildIndex == WorldSaveGameManager.Instance.GetWorldSceneIndex()) {
            Instance.enabled = true;
            EnableInputs();
        }
        else {
            Instance.enabled = false;
            DisableInputs();
        }
    }

    private void OnEnable() {
        if (_inputs == null) {
            _inputs = new();

            // Player & Camera Movement
            _inputs.PlayerMovement.Movement.performed += i => _movementInput = i.ReadValue<Vector2>();
            _inputs.PlayerCamera.Movement.performed += i => _cameraInput = i.ReadValue<Vector2>();
            // Dodge
            _inputs.PlayerActions.Dodge.performed += i => _dodgeInput = true;
            // Sprint
            _inputs.PlayerActions.Sprint.performed += i => _sprintInput = true;
            _inputs.PlayerActions.Sprint.canceled += i => _sprintInput = false;
            // Jump
            _inputs.PlayerActions.Jump.performed += i => _jumpInput = true;
            // Attack Actions
            _inputs.PlayerActions.LightAttack.performed += i => _lightAttackInput = true;
        }

        EnableInputs();
    }

    private void OnDestroy() {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    private void OnApplicationFocus(bool focus) {
        if (enabled) {
            if (focus) {
                EnableInputs();
            }
            else {
                DisableInputs();
            }
        }
    }

    void EnableInputs() {
        _inputs.PlayerMovement.Enable();
        _inputs.PlayerCamera.Enable();
        _inputs.PlayerActions.Enable();
    }

    void DisableInputs() {
        _inputs.PlayerMovement.Disable();
        _inputs.PlayerCamera.Disable();
        _inputs.PlayerActions.Disable();
    }

    private void Update() {
        HandleAllInputs();
    }

    void HandleAllInputs() {
        HandlePlayerMovementInput();
        HandleCameraMovementInput();
        HandleDodgeInput();
        HandleSprintInput();
        HandleJumpInput();
        HandleLightAttackInput();
    }

    // Movement

    void HandlePlayerMovementInput() {

        HorizontalInput = _movementInput.x;
        VerticalInput = _movementInput.y;

        MoveAmount = Mathf.Clamp01(Mathf.Abs(HorizontalInput) + Mathf.Abs(VerticalInput));

        // clamp movement
        if (MoveAmount <= 0.5f && MoveAmount > 0) {
            MoveAmount = 0.5f;
        }
        else if (MoveAmount > 0.5f && MoveAmount <= 1) {
            MoveAmount = 1;
        }

        if(Player == null) { return; }

        // Only pass vertical because horizontal is only for lock on
        Player.PlayerAnimatorManager.UpdateAnimatorMovementParameters(0, MoveAmount, Player.PlayerNetworkManager.IsSprinting.Value);
    }

    void HandleCameraMovementInput() {
        CameraHorizontalInput = _cameraInput.x;
        CameraVerticalInput = _cameraInput.y;
    }

    // Actions

    void HandleDodgeInput() {
        if (_dodgeInput) {
            _dodgeInput = false;

            Player.PlayerMovementManager.AttemptToPerformDodge();
        }
    }

    void HandleSprintInput() {
        if (_sprintInput) {
            Player.PlayerMovementManager.HandleSprint();
        }
        else {
            Player.PlayerNetworkManager.IsSprinting.Value = false;
        }
    }

    void HandleJumpInput() {
        if (_jumpInput) {
            _jumpInput = false;

            Player.PlayerMovementManager.AttemptToPerformJump();
        }
    }

    // Attack Actions

    void HandleLightAttackInput() {
        if (_lightAttackInput) {
            _lightAttackInput = false;

            // TODO: Check For UI

            Player.PlayerNetworkManager.SetCharacterActionHand(true);

            Player.PlayerCombatManager.PerformWeaponBasedAction(Player.PlayerInventoryManager.CurrentRightHandWeapon.OhLightAction, Player.PlayerInventoryManager.CurrentRightHandWeapon);
        }
    }
}
