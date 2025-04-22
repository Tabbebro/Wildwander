using UnityEngine;
using Unity.Netcode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class WorldAIManager : MonoBehaviour
{
    public static WorldAIManager Instance;

    [Header("Characters")]
    [SerializeField] [ReadOnly] List<AICharacterSpawner> _aiCharacterSpawners;
    [SerializeField] List<GameObject> _spawnedCharacters;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    public void SpawnCharacter(AICharacterSpawner aiCharacterSpawner) {
        if (NetworkManager.Singleton.IsServer) {
            _aiCharacterSpawners.Add(aiCharacterSpawner);
            aiCharacterSpawner.AttemptToSpawnCharacter();
        }
    }

    void DespawnAllCharacters() {
        foreach (GameObject character in _spawnedCharacters) {
            character.GetComponent<NetworkObject>().Despawn();
        }
    }
}
