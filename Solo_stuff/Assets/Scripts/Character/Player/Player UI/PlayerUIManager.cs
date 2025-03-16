using UnityEngine;
using Unity.Netcode;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager Instance;

    [Header("NETWORK JOIN")]
    [SerializeField] public bool _startGameAsClient;

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
    private void Update() {
        if (_startGameAsClient) {
            _startGameAsClient = false;
            NetworkManager.Singleton.Shutdown();
            NetworkManager.Singleton.StartClient();
        }
    }

    private void Start() {
        DontDestroyOnLoad(gameObject);
    }

    public void JoinGameAsClient() {
        _startGameAsClient = true;
    }
}
