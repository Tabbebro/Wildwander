using UnityEngine;

public class EventTriggerBossFight : MonoBehaviour
{
    [SerializeField] int _bossID;

    private void OnTriggerEnter(Collider other) {
        AIBossCharacterManager boss = WorldAIManager.Instance.GetBossByID(_bossID);

        if (boss != null) {
            boss.WakeBoss();
        }
    }
}
