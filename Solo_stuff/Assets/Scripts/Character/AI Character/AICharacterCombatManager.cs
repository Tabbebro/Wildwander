using UnityEngine;

public class AICharacterCombatManager : CharacterCombatManager
{

    protected AICharacterManager _aiCharacter;

    [Header("Action Recovery")]
    public float ActionRecoveryTimer = 0;

    [Header("Target Information")]
    public float DistanceFromTarget;
    public float ViewableAngle;
    public Vector3 TargetsDirection;

    [Header("Detection")]
    public float DetectionRadius = 15f;
    public float MinFOV = -35;
    public float MaxFOV = 35;

    [Header("Attack Rotation")]
    public float AttackRotationSpeed = 25;

    protected override void Awake() {
        base.Awake();

        _aiCharacter = GetComponent<AICharacterManager>();
    }

    public void FindTargetLineOfSight(AICharacterManager aiCharacter) {
        if (CurrentTarget != null) {
            return;
        }

        Collider[] colliders = Physics.OverlapSphere(aiCharacter.transform.position, DetectionRadius, WorldUtilityManager.Instance.GetCharacterLayers());

        for (int i = 0; i < colliders.Length; i++) {
            CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

            // Check For Null, Self & Dead
            if (targetCharacter == null) { continue; }
            if (targetCharacter == aiCharacter) { continue; }
            if (targetCharacter.IsDead.Value) { continue; }

            // If Character Can Damage Target Make Them Target
            if (WorldUtilityManager.Instance.CanIDamageThisTarget(aiCharacter.CharacterGroup, targetCharacter.CharacterGroup)) {
                Vector3 targetDirection = targetCharacter.transform.position - aiCharacter.transform.position;
                float AngleOfPotentialTarget = Vector3.Angle(targetDirection, aiCharacter.transform.forward);

                // Check If In FOV
                if (AngleOfPotentialTarget > MinFOV && AngleOfPotentialTarget < MaxFOV) {
                    // Checks If Character Has Clear Line Of Sight
                    if(Physics.Linecast(
                        aiCharacter.CharacterCombatManager.LockOnTransform.position, 
                        targetCharacter.CharacterCombatManager.LockOnTransform.position, 
                        WorldUtilityManager.Instance.GetEnviroLayers())) {

                        Debug.DrawLine(aiCharacter.CharacterCombatManager.LockOnTransform.position, targetCharacter.CharacterCombatManager.LockOnTransform.position, Color.red);
                    }
                    else {
                        TargetsDirection = targetCharacter.transform.position - transform.position;
                        ViewableAngle = WorldUtilityManager.Instance.GetAngleOfTarget(transform, targetDirection);
                        aiCharacter.CharacterCombatManager.SetTarget(targetCharacter);
                        PivotTowardsTarget(aiCharacter);
                    }
                }
            }
        }
    }

    public void PivotTowardsTarget(AICharacterManager aiCharacter) {
        // Play Correct Pivot Animation
        if (aiCharacter.IsPerformingAction) { return; }

        // 45 Angle Pivot
        if (ViewableAngle >= 20 && ViewableAngle <= 60) {
            print("Turning 45 Degrees");
            aiCharacter.CharacterAnimatorManager.PlayTargetActionAnimation("Turn_R45", true);
        }
        else if (ViewableAngle <= -20 && ViewableAngle >= -60) {
            print("Turning 45 Degrees");
            aiCharacter.CharacterAnimatorManager.PlayTargetActionAnimation("Turn_L45", true);
        }
        // 90 Angle Pivot
        else if (ViewableAngle >= 61 && ViewableAngle <= 110) {
            print("Turning 90 Degrees");
            aiCharacter.CharacterAnimatorManager.PlayTargetActionAnimation("Turn_R90", true);
        }
        else if (ViewableAngle <= -61 && ViewableAngle >= -110) {
            print("Turning 90 Degrees");
            aiCharacter.CharacterAnimatorManager.PlayTargetActionAnimation("Turn_L90", true);
        }
        // 135 Angle Pivot
        else if (ViewableAngle >= 110 && ViewableAngle <= 145) {
            print("Turning 135 Degrees");
            aiCharacter.CharacterAnimatorManager.PlayTargetActionAnimation("Turn_L135", true);
        }
        else if (ViewableAngle <= -110 && ViewableAngle >= -145) {
            print("Turning 135 Degrees");
            aiCharacter.CharacterAnimatorManager.PlayTargetActionAnimation("Turn_L135", true);
        }
        // 180 Angle Pivot
        else if (ViewableAngle >= 146 && ViewableAngle <= 180) {
            print("Turning 180 Degrees");
            aiCharacter.CharacterAnimatorManager.PlayTargetActionAnimation("Turn_L180", true);
        }
        else if (ViewableAngle <= -146 && ViewableAngle >= -180) {
            print("Turning 180 Degrees");
            aiCharacter.CharacterAnimatorManager.PlayTargetActionAnimation("Turn_L180", true);
        }
    }

    public void RotateTowardsAgent(AICharacterManager aiCharacter) {
        if (aiCharacter.AICharacterNetworkManager.IsMoving.Value) {
            aiCharacter.transform.rotation = aiCharacter.NavmeshAgent.transform.rotation;
        }
    }

    public void RotateTowardsTarget(AICharacterManager aiCharacter) {
        if (CurrentTarget == null) { return; }

        if (!aiCharacter.CharacterMovementManager.CanRotate) { return; }

        if (!aiCharacter.IsPerformingAction) { return; }

        Vector3 targetDirection = CurrentTarget.transform.position - aiCharacter.transform.position;
        targetDirection.y = 0f;
        targetDirection.Normalize();

        if (targetDirection == Vector3.zero) {
            targetDirection = aiCharacter.transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, targetRotation, AttackRotationSpeed * Time.deltaTime);
    }

    public void HandleActionRecovery(AICharacterManager aiCharacter) {
        if (ActionRecoveryTimer > 0) {
            if (!aiCharacter.IsPerformingAction) {
                ActionRecoveryTimer -= Time.deltaTime;
            }
        }
    }
}
