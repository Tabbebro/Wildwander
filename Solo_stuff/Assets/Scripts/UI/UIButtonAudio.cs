using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonAudio : MonoBehaviour, ISelectHandler 
{
    Button _button;

    private void Awake() {
        _button = GetComponent<Button>();

        if (_button) {
            _button.onClick.AddListener(ButtonOnClick);
        }
    }

    public void OnSelect(BaseEventData eventData) {
        WorldSFXManager.Instance.PlayUIAudio(WorldSFXManager.Instance.SelectButton);
    }

    void ButtonOnClick() {
        WorldSFXManager.Instance.PlayUIAudio(WorldSFXManager.Instance.ClickButton);
    }
}
