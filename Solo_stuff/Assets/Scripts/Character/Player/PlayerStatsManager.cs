using UnityEngine;

public class PlayerStatsManager : CharacterStatsManager
{
    PlayerManager _player;
    protected override void Awake() {
        base.Awake();

        _player = GetComponent<PlayerManager>();

    }

    protected override void Start() {
        base.Start();

        // TODO: Remove When Character Creator Is Done
        CalculateHealthBasedOnLevel(_player.PlayerNetworkManager.Vitality.Value);
        CalculateStaminaBasedOnLevel(_player.PlayerNetworkManager.Endurance.Value);
    }
}
