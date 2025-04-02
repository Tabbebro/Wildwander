using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectOnHighlight : MonoBehaviour, IPointerEnterHandler
{
    Selectable _thisSelectable;
    private void Awake() {
        _thisSelectable = GetComponent<Selectable>();

    }

    public void OnPointerEnter(PointerEventData eventData) {
        _thisSelectable.Select();
    }
}
