using Unity.Netcode;
using UnityEngine;

public class NetworkObjectSpawner : MonoBehaviour
{
    [Header("Object")]
    [SerializeField] GameObject _networkGameObject;
    [SerializeField] GameObject _instantiatedGameObject;

    private void Start() {
        WorldObjectManager.Instance.SpawnObject(this);
        gameObject.SetActive(false);
    }

    public void AttemptToSpawnCharacter() {
        if (_networkGameObject != null) {
            _instantiatedGameObject = Instantiate(_networkGameObject);
            _instantiatedGameObject.transform.position = transform.position;
            _instantiatedGameObject.transform.rotation = transform.rotation;
            _instantiatedGameObject.GetComponent<NetworkObject>().Spawn();
        }
    }
}
