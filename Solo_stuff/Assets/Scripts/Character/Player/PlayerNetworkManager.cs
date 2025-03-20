using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

public class PlayerNetworkManager : CharacterNetworkManager
{
    PlayerManager _player;

    public NetworkVariable<FixedString64Bytes> CharacterName = new NetworkVariable<FixedString64Bytes>("Character", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    protected override void Awake() {
        base.Awake();
        _player = GetComponent<PlayerManager>();
    }

    public void SetNewMaxHealthValue(int oldVitality, int newVitality) {
        MaxHealth.Value = _player.PlayerStatsManager.CalculateHealthBasedOnLevel(newVitality);
        PlayerUIManager.Instance.PlayerUIHudManager.SetMaxHealthValue(MaxHealth.Value);
        CurrentHealth.Value = MaxHealth.Value;
    }

    public void SetNewMaxStaminaValue(int oldEndurance, int newEndurance) {
        MaxStamina.Value = _player.PlayerStatsManager.CalculateStaminaBasedOnLevel(newEndurance);
        PlayerUIManager.Instance.PlayerUIHudManager.SetMaxStaminaValue(MaxStamina.Value);
        CurrentStamina.Value = MaxStamina.Value;
    }
}
