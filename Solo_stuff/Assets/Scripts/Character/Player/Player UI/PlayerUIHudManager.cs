using UnityEngine;
using UnityEngine.UI;

public class PlayerUIHudManager : MonoBehaviour
{
    [Header("Stat Bars")]
    [SerializeField] UI_StatBar _healthBar;
    [SerializeField] UI_StatBar _staminaBar;

    [Header("Quick Slots")]
    [SerializeField] Image _rightWeaponQuickSlotIcon;
    [SerializeField] Image _leftWeaponQuickSlotIcon;

    [Header("Boss Health Bar")]
    public Transform BossHealthBarParent;
    public GameObject BossHealthBarObject;

    public void RefreshHud() {

        _healthBar.gameObject.SetActive(false);
        _healthBar.gameObject.SetActive(true);

        _staminaBar.gameObject.SetActive(false);
        _staminaBar.gameObject.SetActive(true);
    }

    #region Health
    public void SetNewHealthValue(int oldValue, int newValue) {
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

    #region Quick Slots
    public void SetRightWeaponQuickSlotIcon(int weaponID) {
        
        WeaponItem weapon = WorldItemDatabase.Instance.GetWeaponByID(weaponID);
        
        // Check If Weapon ID Is Null
        if (weapon == null) {
            Debug.Log("Item Is Null");
            _rightWeaponQuickSlotIcon.enabled = false;
            _rightWeaponQuickSlotIcon.sprite = null;
            return;
        }
        // Check If item Has Icon
        if (weapon.ItemIcon == null) {
            Debug.Log("Item Has No Icon");
            _rightWeaponQuickSlotIcon.enabled = false;
            _rightWeaponQuickSlotIcon.sprite = null;
            return;
        }

        // TODO: Check Item Requirements

        _rightWeaponQuickSlotIcon.sprite = weapon.ItemIcon;
        _rightWeaponQuickSlotIcon.enabled = true;
    }

    public void SetLeftWeaponQuickSlotIcon(int weaponID) {

        WeaponItem weapon = WorldItemDatabase.Instance.GetWeaponByID(weaponID);

        // Check If Weapon ID Is Null
        if (weapon == null) {
            Debug.Log("Item Is Null");
            _leftWeaponQuickSlotIcon.enabled = false;
            _leftWeaponQuickSlotIcon.sprite = null;
            return;
        }
        // Check If item Has Icon
        if (weapon.ItemIcon == null) {
            Debug.Log("Item Has No Icon");
            _leftWeaponQuickSlotIcon.enabled = false;
            _leftWeaponQuickSlotIcon.sprite = null;
            return;
        }

        // TODO: Check Item Requirements

        _leftWeaponQuickSlotIcon.sprite = weapon.ItemIcon;
        _leftWeaponQuickSlotIcon.enabled = true;
    }
    #endregion
}
