using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterManager : MonoBehaviour
{
    [HideInInspector] public CharacterController _characterController;
    [HideInInspector] public Animator _animator;

    [Header("Is Player?")]
    public bool IsPlayer = false;

    [Header("Flags (Character Manager)")]
    public bool isPerformingAction = false;
    public bool applyRootMotion = false;
    public bool canRotate = true;
    public bool canMove = true;
    public bool isSprinting = false;

    [Header("Stats")]
    public int Endurance = 1;
    public float CurrentStamina { get; private set; }
    public int MaxStamina = 0;


    protected virtual void Awake() {
        DontDestroyOnLoad(this);

        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    protected virtual void Update() {

    }

    protected virtual void LateUpdate() {
        
    }


    public virtual void SetCurrentStamina(float newStamina) {
        float OldStamina = CurrentStamina;
        if (IsPlayer) {
            PlayerUIManager.Instance.PlayerUIHudManager.SetNewStaminaValue(OldStamina, newStamina);
        }
        CurrentStamina = newStamina;
    }

}
