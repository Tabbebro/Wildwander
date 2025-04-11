using UnityEngine;
using Unity.Netcode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class WorldAIManager : MonoBehaviour
{
    public static WorldAIManager Instance;

    [Header("Debug")]
    [SerializeField] bool _despawnCharacters = false;
    [SerializeField] bool _respawnCharacters = false;

    [Header("Characters")]
    [SerializeField] GameObject[] _aiCharacters;
    [SerializeField] List<GameObject> _spawnedCharacters;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        if (NetworkManager.Singleton.IsServer) {
            StartCoroutine(WaitForSceneToLoadToSpawnAI());
        }
    }

    private void Update() {
        if (_despawnCharacters) {
            _despawnCharacters = false;
            DespawnAllCharacters();
        }
        if (_respawnCharacters) { 
            _respawnCharacters = false;
            SpawnAllCharacters();
        }
    }

    IEnumerator WaitForSceneToLoadToSpawnAI() {
        while (!SceneManager.GetActiveScene().isLoaded) {
            yield return null;
        }

        SpawnAllCharacters();
    }

    void SpawnAllCharacters() {
        foreach (GameObject character in _aiCharacters) {
            GameObject instantiatedCharacter = Instantiate(character);
            instantiatedCharacter.GetComponent<NetworkObject>().Spawn();
            _spawnedCharacters.Add(instantiatedCharacter);
        }
    }

    void DespawnAllCharacters() {
        foreach (GameObject character in _spawnedCharacters) {
            character.GetComponent<NetworkObject>().Despawn();
        }
    }
}
