using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public CharacterController _characterController;
    protected virtual void Awake() {
        DontDestroyOnLoad(this);

        _characterController = GetComponent<CharacterController>();
    }

    protected virtual void Update() {

    }
}
