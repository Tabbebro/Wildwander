using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    [SerializeField] ThirdPersonController _controller;

    public void StartDodge() {
        _controller.SetDodgeBool(true);
    }

    public void EndDodge() {
        _controller.SetDodgeBool(false);
    }
}
