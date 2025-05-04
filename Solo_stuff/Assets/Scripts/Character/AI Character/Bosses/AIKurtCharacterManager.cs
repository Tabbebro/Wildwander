using UnityEngine;

public class AIKurtCharacterManager : AIBossCharacterManager
{
    [HideInInspector] public AIKurtCombatManager KurtCombatManager;
    [HideInInspector] public AIKurtSFXManager KurtSoundManager;
    [HideInInspector] public AIKurtVFXManager KurtVFXManager;

    protected override void Awake() {
        base.Awake();

        KurtCombatManager = GetComponent<AIKurtCombatManager>();
        KurtSoundManager = GetComponent<AIKurtSFXManager>();
        KurtVFXManager = GetComponent<AIKurtVFXManager>();
    }
}
