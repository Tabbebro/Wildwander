using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectOnHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Selectable _thisSelectable;
    private void Awake() {
        _thisSelectable = GetComponent<Selectable>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        _thisSelectable.Select();
    }

    public void OnPointerExit(PointerEventData eventData) {
        EventSystem.current.SetSelectedGameObject(null);
    }


    
}
