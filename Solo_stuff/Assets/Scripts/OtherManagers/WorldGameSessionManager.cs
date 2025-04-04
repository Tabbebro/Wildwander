using System.Collections.Generic;
using UnityEngine;


public class WorldGameSessionManager : MonoBehaviour
{

    public static WorldGameSessionManager Instance;

    [Header("Active Players In Session")]
    public List<PlayerManager> Players = new();

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    public void AddPlayerToActivePlayersList(PlayerManager player) {

        // Adds Player To The List
        if (!Players.Contains(player)) {
            Players.Add(player);
        }

        // Checks For Null Slots
        for (int i = Players.Count - 1; i > -1; i--) {
            if (Players[i] == null) {
                Players.RemoveAt(i);
            }
        }
    }

    public void RemovePlayerToActivePlayersList(PlayerManager player) {

        // Removes Player From The List
        if (Players.Contains(player)) {
            Players.Remove(player);
        }

        // Checks For Null Slots
        for (int i = Players.Count - 1; i > -1; i--) {
            if (Players[i] == null) {
                Players.RemoveAt(i);
            }
        }
    }
}
