using UnityEngine;

public class AICharacterManager : CharacterManager
{
    [HideInInspector] public AICharacterCombatManager AICharacterCombatManager;

    [Header("Current State")]
    [SerializeField] AIState _currentState;

    protected override void Awake() {
        base.Awake();
        AICharacterCombatManager = GetComponent<AICharacterCombatManager>();
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();

        ProcessStateMachine();
    }

    void ProcessStateMachine() {

        AIState nextState = null;

        if (_currentState != null) {
            nextState = _currentState.Tick(this);  
        }

        if (nextState != null) { 
            _currentState = nextState;
        }
    }
}
