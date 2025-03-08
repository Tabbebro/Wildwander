using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Match_Scroll_Whee_To_Selected_Button : MonoBehaviour
{
    [SerializeField] GameObject _currentSelected;
    [SerializeField] GameObject _previouslySelected;
    [SerializeField] RectTransform _currentSelectedTransform;

    [SerializeField] RectTransform _contentPanel;
    [SerializeField] ScrollRect _scrollRect;

    private void Update() {
        _currentSelected = EventSystem.current.currentSelectedGameObject;

        if (_currentSelected != null) {
            if(_currentSelected != _previouslySelected) {
                _previouslySelected = _currentSelected;
                _currentSelectedTransform = _currentSelected.GetComponent<RectTransform>();
                SnapTo(_currentSelectedTransform);
            }
        }
    }

    void SnapTo(RectTransform target) {
        Canvas.ForceUpdateCanvases();

        Vector2 newPosition = (Vector2)_scrollRect.transform.InverseTransformPoint(_contentPanel.position) - (Vector2)_scrollRect.transform.InverseTransformPoint(target.position);

        newPosition.x = 0;

        _contentPanel.anchoredPosition = newPosition;
    }
}
