using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    [SerializeField] Vector3 Offset;
    private void Start() {
        Offset += transform.position;
        if (PlayerInputManager.Instance.Player != null) { 
            PlayerInputManager.Instance.Player.transform.position = Offset;
            PlayerInputManager.Instance.Player.CharacterController.enabled = true;
        }
    }
}
