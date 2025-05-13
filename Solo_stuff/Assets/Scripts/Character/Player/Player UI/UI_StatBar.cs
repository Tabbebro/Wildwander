using UnityEngine;
using UnityEngine.UI;

public class UI_StatBar : MonoBehaviour
{
    protected Slider _slider;
    protected RectTransform _rectTransform;

    [Header("Bar Options")]
    [SerializeField] protected bool _scaleBarWithStats = true;
    [SerializeField] protected float _widthScaleMultiplier = 1;


    protected virtual void Awake() {
        _slider = GetComponent<Slider>();
        _rectTransform = GetComponent<RectTransform>();
    }

    protected virtual void Start() {

    }

    public virtual void SetStat(int newValue) {
        _slider.value = newValue;
    }

    public virtual void SetMaxStat(int maxValue) {
        _slider.maxValue = maxValue;
        _slider.value = maxValue;

        if (_scaleBarWithStats) {
            _rectTransform.sizeDelta = new Vector2(maxValue * _widthScaleMultiplier, _rectTransform.sizeDelta.y);

            // Resets Bar Position
            PlayerUIManager.Instance.PlayerUIHudManager.RefreshHud();
        }
    }

}
