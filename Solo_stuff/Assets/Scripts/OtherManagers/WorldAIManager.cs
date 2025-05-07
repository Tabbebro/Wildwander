using UnityEngine;
using Unity.Netcode;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WorldAIManager : MonoBehaviour
{
    public static WorldAIManager Instance;

    [Header("Characters")]
    [SerializeField] [ReadOnly] List<AICharacterSpawner> _aiCharacterSpawners;
    [SerializeField] List<AICharacterManager> _spawnedCharacters;

    [Header("Characters")]
    [SerializeField] List<AIBossCharacterManager> _spawnedBosses;

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

    public void AddCharacterToSpawnedCharacters(AICharacterManager characterObject) {
        if (_spawnedCharacters.Contains(characterObject)) { return; }
        _spawnedCharacters.Add(characterObject);

        AIBossCharacterManager bossCharacter = characterObject as AIBossCharacterManager;
        if (bossCharacter != null) {
            if (_spawnedBosses.Contains(bossCharacter)) { return; }
            _spawnedBosses.Add(bossCharacter);
        }
    }

    public AIBossCharacterManager GetBossByID(int id) {
        return _spawnedBosses.FirstOrDefault(boss => boss.BossID == id);
    }

    void DespawnAllCharacters() {
        foreach (AICharacterManager character in _spawnedCharacters) {
            character.GetComponent<NetworkObject>().Despawn();
        }
    }
}
