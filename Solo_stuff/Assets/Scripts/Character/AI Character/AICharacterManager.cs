using UnityEngine;
using UnityEngine.AI;

public class AICharacterManager : CharacterManager
{

    [HideInInspector] public AICharacterNetworkManager AICharacterNetworkManager;
    [HideInInspector] public AICharacterMovementManager AICharacterMovementManager;
    [HideInInspector] public AICharacterCombatManager AICharacterCombatManager;

    [Header("Character Name")]
    public string CharacterName = "";

    [Header("Navmesh Agent")]
    public NavMeshAgent NavmeshAgent;

    [Header("Current State")]
    [SerializeField] protected AIState _currentState;

    [Header("States")]
    public IdleState Idle;
    public PursueTargetState PursueTarget;
    public CombatStanceState CombatStance;
    public AttackState Attack;

    protected override void Awake() {
        base.Awake();

        AICharacterNetworkManager = GetComponent<AICharacterNetworkManager>();
        AICharacterMovementManager = GetComponent<AICharacterMovementManager>();
        AICharacterCombatManager = GetComponent<AICharacterCombatManager>();

        NavmeshAgent = GetComponentInChildren<NavMeshAgent>();
    }

    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();
        if (IsOwner) {
            // Using Copy Of Scriptable Objects
            Idle = Instantiate(Idle);
            PursueTarget = Instantiate(PursueTarget);
            CombatStance = Instantiate(CombatStance);
            Attack = Instantiate(Attack);
            _currentState = Idle;
        }

        AICharacterNetworkManager.CurrentHealth.OnValueChanged += AICharacterNetworkManager.CheckHP;
    }

    public override void OnNetworkDespawn() {
        base.OnNetworkDespawn();
        AICharacterNetworkManager.CurrentHealth.OnValueChanged -= AICharacterNetworkManager.CheckHP;
    }

    protected override void OnEnable() {
        base.OnEnable();
        if (CharacterUIManager.HasFloatingUIBar) {
            CharacterNetworkManager.CurrentHealth.OnValueChanged += CharacterUIManager.OnHPChanged;
        }
    }

    protected override void OnDisable() {
        base.OnDisable();
        if (CharacterUIManager.HasFloatingUIBar) {
            CharacterNetworkManager.CurrentHealth.OnValueChanged -= CharacterUIManager.OnHPChanged;
        }
    }

    protected override void Update() {
        base.Update();

        AICharacterCombatManager.HandleActionRecovery(this);
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();

        if (IsOwner) {
            ProcessStateMachine();
        }
    }

    void ProcessStateMachine() {

        AIState nextState = null;

        if (_currentState != null) {
            nextState = _currentState.Tick(this);  
        }

        if (nextState != null) { 
            _currentState = nextState;
        }

        
        NavmeshAgent.transform.localPosition = Vector3.zero;
        NavmeshAgent.transform.localRotation = Quaternion.identity;

        if (AICharacterCombatManager.CurrentTarget != null) {
            AICharacterCombatManager.TargetsDirection = AICharacterCombatManager.CurrentTarget.transform.position - transform.position;
            AICharacterCombatManager.ViewableAngle = WorldUtilityManager.Instance.GetAngleOfTarget(transform, AICharacterCombatManager.TargetsDirection);
            AICharacterCombatManager.DistanceFromTarget = Vector3.Distance(transform.position, AICharacterCombatManager.CurrentTarget.transform.position);
        }

        if (NavmeshAgent.enabled) {
            Vector3 agentDestination = NavmeshAgent.destination;
            float remainingDistance = Vector3.Distance(agentDestination, transform.position);
            if (remainingDistance > NavmeshAgent.stoppingDistance) {
                AICharacterNetworkManager.IsMoving.Value = true;   
            }
            else {
                AICharacterNetworkManager.IsMoving.Value = false;   
            }
        }
        else {
            AICharacterNetworkManager.IsMoving.Value = false;   
        }
    }
}
