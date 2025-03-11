using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NeverLoseSelection : MonoBehaviour
{
    Selectable _selectable;

    private void Awake() {
        _selectable = GetComponent<Selectable>();
    }

    private void Update() {
        if (_selectable != EventSystem.current.GetComponent<Selectable>()) {
            _selectable.Select();
        }
    }
}
