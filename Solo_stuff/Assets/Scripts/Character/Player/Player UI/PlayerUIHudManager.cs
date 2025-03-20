using UnityEngine;

public class PlayerUIHudManager : MonoBehaviour
{
    [SerializeField] UI_StatBar _healthBar;
    [SerializeField] UI_StatBar _staminaBar;

    public void RefreshHud() {

        _healthBar.gameObject.SetActive(false);
        _healthBar.gameObject.SetActive(true);

        _staminaBar.gameObject.SetActive(false);
        _staminaBar.gameObject.SetActive(true);
    }

    #region Health
    public void SetNewHealthValue(int oldValue, int newValue) {
        print("Current Health Changed From: " + oldValue + " To: " + newValue);
        _healthBar.SetStat(newValue);
    }

    public void SetMaxHealthValue(int maxHealth) {
        _healthBar.SetMaxStat(maxHealth);
    }
    #endregion


    #region Stamina
    public void SetNewStaminaValue(float oldValue, float newValue) {
        _staminaBar.SetStat(Mathf.RoundToInt(newValue));
    }

    public void SetMaxStaminaValue(int maxStamina) {
        _staminaBar.SetMaxStat(maxStamina);
    }
    #endregion

}
