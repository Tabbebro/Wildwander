using UnityEngine;
using Unity.Netcode;

public class AICharacterSpawner : MonoBehaviour
{
    [Header("Character")]
    [SerializeField] GameObject _characterGameObject;
    [SerializeField] GameObject _instantiatedGameObject;

    private void Start() {
        WorldAIManager.Instance.SpawnCharacter(this);
        gameObject.SetActive(false);
    }

    public void AttemptToSpawnCharacter() {
        if (_characterGameObject != null) {
            _instantiatedGameObject = Instantiate(_characterGameObject);
            _instantiatedGameObject.transform.position = transform.position;
            _instantiatedGameObject.transform.rotation = transform.rotation;
            _instantiatedGameObject.GetComponent<NetworkObject>().Spawn();
            WorldAIManager.Instance.AddCharacterToSpawnedCharacters(_instantiatedGameObject.GetComponent<AICharacterManager>());
        }
    }
}
