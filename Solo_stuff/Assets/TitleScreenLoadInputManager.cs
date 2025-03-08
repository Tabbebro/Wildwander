using UnityEngine;

public class TitleScreenLoadInputManager : MonoBehaviour
{
    Inputs _inputs;

    [Header("Title Screen Inputs")]
    [SerializeField] bool _deleteCharacterSlot = false;

    private void Update() {
        if (_deleteCharacterSlot) {
            _deleteCharacterSlot = false;
            TitleScreenManager.Instance.AttemptToDeleteCharacterSlot();
        }
    }

    private void OnEnable() {
        if(_inputs == null) {
            _inputs = new();
            _inputs.UI.ButtonNorth.performed += i => _deleteCharacterSlot = true;
        }

        _inputs.UI.Enable();
    }

    private void OnDisable() {
        
        _inputs.UI.Disable();
    }
}
