using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Attack Action")]
public class AICharacterAttackAction : ScriptableObject {

    [Header("Attack")]
    [SerializeField] string _attackAnimation;

    [Header("Combo Action")]
    public AICharacterAttackAction ComboAction;

    [Header("Action Values")]
    [SerializeField] AttackType _attackType;
    public int AttackWeight = 50;
    public float ActionRecoveryTime = 1.5f;
    public float MinAttackAngle = -35;
    public float MaxAttackAngle = 35;
    public float MinAttackDistance = 0;
    public float MaxAttackDistance = 2;

    public void AttemptToPerformAction(AICharacterManager aiCharacter) {
        aiCharacter.CharacterAnimatorManager.PlayTargetActionAnimation(_attackAnimation, true);
    }
}
