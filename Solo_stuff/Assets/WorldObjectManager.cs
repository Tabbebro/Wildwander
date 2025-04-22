using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class WorldObjectManager : MonoBehaviour
{
    public static WorldObjectManager Instance;

    [Header("Objects")]
    [SerializeField][ReadOnly] List<NetworkObjectSpawner> _networkObjectSpawners;
    [SerializeField] List<GameObject> _spawnedObjects;

    [Header("Fog Walls")]
    public List<FogWallInteractable> FogWalls;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    public void SpawnObject(NetworkObjectSpawner networkObjectSpawner) {
        if (NetworkManager.Singleton.IsServer) {
            _networkObjectSpawners.Add(networkObjectSpawner);
            networkObjectSpawner.AttemptToSpawnCharacter();
        }
    }

    void DespawnAllObjects() {
        foreach (GameObject character in _spawnedObjects) {
            character.GetComponent<NetworkObject>().Despawn();
        }
    }

    public void AddFogWallToList(FogWallInteractable fogWall) {
        if (!FogWalls.Contains(fogWall)) {
            FogWalls.Add(fogWall);
        }
    }

    public void RemoveFogWallFromList(FogWallInteractable fogWall) {
        if (FogWalls.Contains(fogWall)) {
            FogWalls.Remove(fogWall);
        }
    }
}
