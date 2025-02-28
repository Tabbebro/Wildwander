using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterManager : MonoBehaviour
{
    [HideInInspector] public CharacterController _characterController;
    [HideInInspector] public Animator _animator;

    [Header("Flags")]
    public bool isPerformingAction = false;
    public bool applyRootMotion = false;
    public bool canRotate = true;
    public bool canMove = true;

    [Header("Impact Variables")]
    Vector3 _impact = Vector3.zero;
    float _mass = 3;

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
