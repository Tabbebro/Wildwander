using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera Instance;

    public Camera CameraObject;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        DontDestroyOnLoad(gameObject);
    }
}
