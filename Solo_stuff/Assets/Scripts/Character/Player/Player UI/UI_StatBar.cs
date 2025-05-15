using UnityEngine;
using UnityEngine.UI;

public class UI_StatBar : MonoBehaviour
{
    protected Slider _slider;
    protected RectTransform _rectTransform;

    [Header("Bar Options")]
    [SerializeField] protected bool _scaleBarWithStats = true;
    [SerializeField] protected float _widthScaleMultiplier = 1;

    [Header("Secondary Bar")]
    [SerializeField] Slider _secondaryBar;
    [SerializeField] bool _showSecondaryBar = true;
    [SerializeField] float _secondaryBarDelay = 1f;
    [SerializeField][ReadOnly] float _secondaryBarTimer = 0f;
    [SerializeField][ReadOnly] bool _updateSecondaryBar = false;


    protected virtual void Awake() {
        _slider = GetComponent<Slider>();
        _rectTransform = GetComponent<RectTransform>();

        ToggleSecondaryBar();
    }

    protected virtual void Start() {

    }

    protected virtual void Update() {
        SecondaryBarUpdate();
    }

    protected virtual void OnEnable() {
        SecondaryBarSetMaxStat((int)_slider.maxValue);
    }

    public virtual void SetStat(int newValue) {
        ResetSecondaryBarTimer(_slider.value, newValue);
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

        SecondaryBarSetMaxStat(maxValue);
    }

    #region Secondary Bar
    void ToggleSecondaryBar() {

        if (_secondaryBar == null) { return; }

        if (_showSecondaryBar) {
            _secondaryBar.gameObject.SetActive(true);
        }
        else {
            _secondaryBar.gameObject.SetActive(false);
        }
    }

    void SecondaryBarSetMaxStat(int maxValue) {
        if (_secondaryBar == null) { return; }

        if (_secondaryBar.maxValue != maxValue) {
            _secondaryBar.maxValue = maxValue;
            _secondaryBar.value = maxValue;            
        }
    }

    void SecondaryBarUpdate() {
        if (_secondaryBar == null) { return; }

        // Deplete Secondary Bar
        _secondaryBarTimer += Time.deltaTime;
        if (_secondaryBarTimer >= _secondaryBarDelay && _secondaryBar.value >= _slider.value && _updateSecondaryBar) {
            _secondaryBar.value = Mathf.MoveTowards(_secondaryBar.value, _slider.value, _secondaryBar.maxValue * 0.5f * Time.deltaTime);
        }

        if (_secondaryBar.value < _slider.value) {
            _secondaryBar.value = _slider.value;
            _updateSecondaryBar = false;
        }
    }

    protected void ResetSecondaryBarTimer(float oldValue, float newValue) {
        if (oldValue > newValue) {
            _secondaryBarTimer = 0;
            _updateSecondaryBar = true;
        }
    }
    #endregion

}
