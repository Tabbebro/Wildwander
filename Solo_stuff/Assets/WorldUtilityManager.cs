using UnityEngine;

public class WorldUtilityManager : MonoBehaviour
{
    public static WorldUtilityManager Instance;

    [Header("Layers")]
    [SerializeField] LayerMask _characterLayers;
    [SerializeField] LayerMask _enviroLayers;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    public LayerMask GetCharacterLayers() {
        return _characterLayers;
    }

    public LayerMask GetEnviroLayers() {
        return _enviroLayers;
    }
}
