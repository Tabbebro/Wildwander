using UnityEngine;
using TMPro;

public class UI_BossHpBar : UI_StatBar
{
    [Header("Boss Character")]
    [SerializeField] AIBossCharacterManager _bossCharacter;

    [Header("Boss Name Text")]
    [SerializeField] TextMeshProUGUI _bossNameText;

    public void EnableBossHPBar(AIBossCharacterManager boss) {
        _bossCharacter = boss;
        _bossCharacter.AICharacterNetworkManager.CurrentHealth.OnValueChanged += OnBossHPChanged;
        SetMaxStat(_bossCharacter.CharacterNetworkManager.MaxHealth.Value);
        SetStat(_bossCharacter.CharacterNetworkManager.CurrentHealth.Value);
        _bossNameText.text = _bossCharacter.CharacterName;
    }

    private void OnDestroy() { 
        _bossCharacter.AICharacterNetworkManager.CurrentHealth.OnValueChanged -= OnBossHPChanged;
    }

    void OnBossHPChanged(int oldValue, int newValue) {
        SetStat(newValue);

        if (newValue <= 0) {
            RemoveHPBar(2.5f);
        }
    }

    public void RemoveHPBar(float time) {
        Destroy(gameObject.transform.parent, time);
    }
}
