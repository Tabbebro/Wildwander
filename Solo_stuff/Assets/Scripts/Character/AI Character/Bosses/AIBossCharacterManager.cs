using UnityEngine;
using Unity.Netcode;
using System.Collections;
using System.Collections.Generic;

public class AIBossCharacterManager : AICharacterManager
{
    public int BossID = 0;
    [SerializeField] bool _hasBeenDefeated = false;


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

                if (_hasBeenDefeated) {
                    AICharacterNetworkManager.IsActive.Value = false;
                }
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
}
