using UnityEngine;

public class CharacterUIManager : MonoBehaviour
{
    [Header("UI")]
    public bool HasFloatingUIBar = true;
    public UI_CharacterHpBar CharacterHPBar;

    public void OnHPChanged(int oldValue, int newValue) {
        CharacterHPBar.OldHealthValue = oldValue;
        CharacterHPBar.SetStat(newValue);
    }
}
