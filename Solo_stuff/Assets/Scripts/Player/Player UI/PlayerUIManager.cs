using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager Instance;

    [HideInInspector] public PlayerUIHudManager PlayerUIHudManager;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else { 
            Destroy(gameObject);
        }

        PlayerUIHudManager = GetComponentInChildren<PlayerUIHudManager>();
    }

    private void Start() {
        DontDestroyOnLoad(gameObject);
    }
}
