using UnityEngine;

public class PlayerManager : CharacterManager
{
    PlayerMovementManager _movement;

    protected override void Awake() {
        base.Awake();

        _movement = GetComponent<PlayerMovementManager>();
    }

    protected override void Update() {
        base.Update();

        _movement.HandleAllMovement();
    }
}
