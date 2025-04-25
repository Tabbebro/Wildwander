using UnityEngine;

public class AIKurtCharacterManager : AIBossCharacterManager
{
    [HideInInspector] public AIKurtCombatManager KurtCombatManager;
    [HideInInspector] public AIKurtSFXManager KurtSoundManager;

    protected override void Awake() {
        base.Awake();

        KurtCombatManager = GetComponent<AIKurtCombatManager>();
        KurtSoundManager = GetComponent<AIKurtSFXManager>();
    }
}
