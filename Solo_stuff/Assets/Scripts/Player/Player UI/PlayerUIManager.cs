using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager Instance;

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
