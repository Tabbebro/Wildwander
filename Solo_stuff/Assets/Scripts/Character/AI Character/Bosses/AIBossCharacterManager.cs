using UnityEngine;
using Unity.Netcode;
using System.Collections;
using System.Collections.Generic;

public class AIBossCharacterManager : AICharacterManager
{
    public int BossID = 0;
    [SerializeField] bool _hasBeenDefeated = false;
    [SerializeField] bool _hasBeenAwakened = false;
    [SerializeField] List<FogWallInteractable> _fogWalls;

    [Header("Debug")]
    [SerializeField] bool _wakeBossUp = false;

    protected override void Update() {
        base.Update();

        if (_wakeBossUp) {
            _wakeBossUp = false;

            WakeBoss();
        }
    }

    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();

        if (IsServer) {
            // If No Data From Boss Add It
            if (!WorldSaveGameManager.Instance.CurrentCharacterData.BossesAwakened.ContainsKey(BossID)) {
                WorldSaveGameManager.Instance.CurrentCharacterData.BossesAwakened.Add(BossID, false);
                WorldSaveGameManager.Instance.CurrentCharacterData.BossesDefeated.Add(BossID, false);
            }
            else {
                _hasBeenDefeated = WorldSaveGameManager.Instance.CurrentCharacterData.BossesDefeated[BossID];
                _hasBeenAwakened = WorldSaveGameManager.Instance.CurrentCharacterData.BossesAwakened[BossID];


            }

            // Get Boss FogWalls
            StartCoroutine(GetFogWalls());

            // Check If Boss Has Been Awakened & Activate FogWalls
            if (_hasBeenAwakened) {
                for (int i = 0; i < _fogWalls.Count; i++) {
                    _fogWalls[i].IsActive.Value = true;
                }
            }

            // Check If Boss Has Been Defeated & Remove Fog Walls
            if (_hasBeenDefeated) {
                for (int i = 0; i < _fogWalls.Count; i++) {
                    _fogWalls[i].IsActive.Value = false;
                }
                AICharacterNetworkManager.IsActive.Value = false;
            }
        }
    }

    IEnumerator GetFogWalls() {
        while (WorldObjectManager.Instance.FogWalls.Count == 0) {
            yield return new WaitForEndOfFrame();
        }

        _fogWalls = new();

        foreach (FogWallInteractable fogWall in WorldObjectManager.Instance.FogWalls) {
            if (fogWall.FogWallID == BossID) {
                _fogWalls.Add(fogWall);
            }
        }
    }

    public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false) {
        if (IsOwner) {
            if (!manuallySelectDeathAnimation) {
                CharacterAnimatorManager.PlayTargetActionAnimation("Death_01", true);
            }
            CharacterNetworkManager.CurrentHealth.Value = 0;
            IsDead.Value = true;

            _hasBeenDefeated = true;
            if (!WorldSaveGameManager.Instance.CurrentCharacterData.BossesAwakened.ContainsKey(BossID)) {
                WorldSaveGameManager.Instance.CurrentCharacterData.BossesAwakened.Add(BossID, true);
                WorldSaveGameManager.Instance.CurrentCharacterData.BossesDefeated.Add(BossID, true);
            }
            else {
                WorldSaveGameManager.Instance.CurrentCharacterData.BossesAwakened.Remove(BossID);
                WorldSaveGameManager.Instance.CurrentCharacterData.BossesDefeated.Remove(BossID);

                WorldSaveGameManager.Instance.CurrentCharacterData.BossesAwakened.Add(BossID, true);
                WorldSaveGameManager.Instance.CurrentCharacterData.BossesDefeated.Add(BossID, true);
            }

            WorldSaveGameManager.Instance.SaveGame();
        }


        // Play Death SFX

        yield return new WaitForSeconds(5);

        // TODO: Give Some Currency On Enemy Death

        // TODO: Disable Character
    }

    public void WakeBoss() {
        _hasBeenAwakened = true;


        if (!WorldSaveGameManager.Instance.CurrentCharacterData.BossesAwakened.ContainsKey(BossID)) {
            WorldSaveGameManager.Instance.CurrentCharacterData.BossesAwakened.Add(BossID, true);
        }
        else {
            WorldSaveGameManager.Instance.CurrentCharacterData.BossesAwakened.Remove(BossID);
            WorldSaveGameManager.Instance.CurrentCharacterData.BossesAwakened.Add(BossID, true);
        }

        for (int i = 0; i < _fogWalls.Count; i++) {
            _fogWalls[i].IsActive.Value = true;
        }
    }
}
