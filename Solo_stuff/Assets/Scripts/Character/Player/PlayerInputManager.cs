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

    [Header("Lock On Input")]
    [SerializeField] bool _lockOnInput = false;
    [SerializeField] bool _lockOnLeftInput = false;
    [SerializeField] bool _lockOnRightInput = false;
    Coroutine _lockOnCoroutine;
    

    [Header("Player Action Input")]
    [SerializeField] bool _dodgeInput = false;
    [SerializeField] bool _sprintInput = false;
    [SerializeField] bool _jumpInput = false;
    [SerializeField] bool _switchRightWeaponInput = false;
    [SerializeField] bool _switchLeftWeaponInput = false;

    [Header("Light Attack Inputs")]
    [SerializeField] bool _lightAttackInput = false;

    [Header("Heavy Attack Inputs")]
    [SerializeField] bool _heavyAttackInput = false;
    [SerializeField] bool _heavyAttackHoldInput = false;


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
            // Weapon Switching
            _inputs.PlayerActions.SwitchRightWeapon.performed += i => _switchRightWeaponInput = true;
            _inputs.PlayerActions.SwitchLeftWeapon.performed += i => _switchLeftWeaponInput = true;
            // Light Attack Actions
            _inputs.PlayerActions.LightAttack.performed += i => _lightAttackInput = true;
            // Heavy Attack Actions
            _inputs.PlayerActions.HeavyAttack.performed += i => _heavyAttackInput = true;
            _inputs.PlayerActions.HeavyAttackHold.performed += i => _heavyAttackHoldInput = true;
            _inputs.PlayerActions.HeavyAttackHold.canceled += i => _heavyAttackHoldInput = false;
            // Lock On
            _inputs.PlayerActions.LockOn.performed += i => _lockOnInput = true;
            _inputs.PlayerActions.ChangeLockOnLeft.performed += i => _lockOnLeftInput = true;
            _inputs.PlayerActions.ChangeLockOnRight.performed += i => _lockOnRightInput = true;
            
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
        HandleLockOnInput();
        HandleLockOnSwitchTargetInput();
        HandlePlayerMovementInput();
        HandleCameraMovementInput();
        HandleDodgeInput();
        HandleSprintInput();
        HandleJumpInput();
        HandleLightAttackInput();
        HandleHeavyAttackInput();
        HandleHeavyAttackHoldInput();
        HandleSwitchRightWeaponInput();
        HandleSwitchLeftWeaponInput();
    }

    // Lock On
    void HandleLockOnInput() {
        // Check For Dead Target
        if (Player.PlayerNetworkManager.IsLockedOn.Value) {
            if (Player.PlayerCombatManager.CurrentTarget == null) { return; }

            if (Player.PlayerCombatManager.CurrentTarget.IsDead.Value) {
                Player.PlayerNetworkManager.IsLockedOn.Value = false;

                // Prevents Multiple Coroutines From Running
                if (_lockOnCoroutine != null) {
                    StopCoroutine(_lockOnCoroutine);
                }

                _lockOnCoroutine = StartCoroutine(PlayerCamera.Instance.WaitThenFindNewTarget());
            }
        }
        // Chekc If Already Locked On
        if (_lockOnInput && Player.PlayerNetworkManager.IsLockedOn.Value) {
            _lockOnInput = false;
            PlayerCamera.Instance.ClearLockOnTargets();
            Player.PlayerNetworkManager.IsLockedOn.Value = false;
            // TODO: Testing
            Player.PlayerCombatManager.SetTarget(null);

            return;
        }
        // Chekc If Not Already Locked On Then Lock On
        if (_lockOnInput && !Player.PlayerNetworkManager.IsLockedOn.Value) {
            _lockOnInput = false;
            // TODO: If Using Ranged Weapon Return
            PlayerCamera.Instance.HandleLocatingLockOnTargets();

            if (PlayerCamera.Instance.NearestLockOnTarget != null) {
                Player.PlayerCombatManager.SetTarget(PlayerCamera.Instance.NearestLockOnTarget);
                Player.PlayerNetworkManager.IsLockedOn.Value = true;
            }
        }
    }

    void HandleLockOnSwitchTargetInput() {
        // Left Input
        if (_lockOnLeftInput) {
            _lockOnLeftInput = false;

            if (Player.PlayerNetworkManager.IsLockedOn.Value) {
                PlayerCamera.Instance.HandleLocatingLockOnTargets();

                if(PlayerCamera.Instance.LeftLockOnTarget != null) {
                    Player.PlayerCombatManager.SetTarget(PlayerCamera.Instance.LeftLockOnTarget);
                }
            }
        }
        // Right Input
        if (_lockOnRightInput) {
            _lockOnRightInput = false;

            if (Player.PlayerNetworkManager.IsLockedOn.Value) {
                PlayerCamera.Instance.HandleLocatingLockOnTargets();

                if (PlayerCamera.Instance.RightLockOnTarget != null) {
                    Player.PlayerCombatManager.SetTarget(PlayerCamera.Instance.RightLockOnTarget);
                }
            }
        }
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

        if (MoveAmount != 0) {
            Player.PlayerNetworkManager.IsMoving.Value = true;
        }
        else {
            Player.PlayerNetworkManager.IsMoving.Value = false;
        }

        // Normal Movement Animations
        if (!Player.PlayerNetworkManager.IsLockedOn.Value || Player.PlayerNetworkManager.IsSprinting.Value) {
            Player.PlayerAnimatorManager.UpdateAnimatorMovementParameters(0, MoveAmount, Player.PlayerNetworkManager.IsSprinting.Value);
        }
        // Strafing Movement Animations
        else {
            Player.PlayerAnimatorManager.UpdateAnimatorMovementParameters(HorizontalInput, VerticalInput, Player.PlayerNetworkManager.IsSprinting.Value);
        }

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

    void HandleHeavyAttackInput() {
        if (_heavyAttackInput) {
            _heavyAttackInput = false;

            // TODO: Check For UI

            Player.PlayerNetworkManager.SetCharacterActionHand(true);

            Player.PlayerCombatManager.PerformWeaponBasedAction(Player.PlayerInventoryManager.CurrentRightHandWeapon.OhHeavyAction, Player.PlayerInventoryManager.CurrentRightHandWeapon);
        }
    }

    void HandleHeavyAttackHoldInput() {
        if (!Player.IsPerformingAction) { return; }
        if (!Player.PlayerNetworkManager.IsUsingRightHand.Value) { return; }

        Player.PlayerNetworkManager.IsChargingAttack.Value = _heavyAttackHoldInput;
    }

    // Weapon Switch Action

    void HandleSwitchRightWeaponInput() {
        if (_switchRightWeaponInput) {
            _switchRightWeaponInput = false;

            Player.PlayerEquipmentManager.SwitchRightWeapon();
        }
    }

    void HandleSwitchLeftWeaponInput() {
        if (_switchLeftWeaponInput) {
            _switchLeftWeaponInput = false;

            Player.PlayerEquipmentManager.SwitchLeftWeapon();
        }
    }
}
