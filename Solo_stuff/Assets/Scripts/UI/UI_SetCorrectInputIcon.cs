using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UI_SetCorrectInputIcon : MonoBehaviour
{
    [SerializeField] Image _image;

    [SerializeField] InputActionReference _input;



    private void Awake() {
        PlayerInputManager.Instance.ChangeUIControllerIconEvent += ChangeSprite;
    }
    private void OnDestroy() {
        PlayerInputManager.Instance.ChangeUIControllerIconEvent -= ChangeSprite;
    }

    private void OnEnable() {
        ChangeSprite(PlayerInputManager.Instance.UIInputDatabase, PlayerInputManager.Instance.ControlScheme, PlayerInputManager.Instance.ControllerType);
    }

    void ChangeSprite(InputIconDatabase database, ControlScheme scheme, ControllerType type) {

        var mapping = database.Mappings.Find(m => m.ActionName == _input.action.name);

        switch (scheme) {
            case ControlScheme.Keyboard:
                _image.sprite = mapping.KeyboardIcon;
                break;
            case ControlScheme.Controller:
                switch (type) {
                    case ControllerType.Playstation:
                        _image.sprite = mapping.PlaystationIcon;
                        break;
                    case ControllerType.Xbox:
                        _image.sprite = mapping.XboxIcon;
                        break;
                }
            break;
        }
    }
}
