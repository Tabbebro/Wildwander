using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterManager : MonoBehaviour
{
    [HideInInspector] public CharacterController _characterController;
    [HideInInspector] public Animator _animator;

    [Header("Flags (Character Manager)")]
    public bool isPerformingAction = false;
    public bool applyRootMotion = false;
    public bool canRotate = true;
    public bool canMove = true;

    protected virtual void Awake() {
        DontDestroyOnLoad(this);

        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    protected virtual void Update() {

    }

    protected virtual void LateUpdate() {
        
    }


}
