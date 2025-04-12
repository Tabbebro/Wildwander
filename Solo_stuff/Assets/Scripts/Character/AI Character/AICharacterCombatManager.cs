using UnityEngine;

public class AICharacterCombatManager : CharacterCombatManager
{
    [Header("Detection")]
    [SerializeField] float _detectionRadius = 15f;
    [SerializeField] float _minDetectionAngle = -35;
    [SerializeField] float _maxDetectionAngle = 35;

    public void FindTargetLineOfSight(AICharacterManager aiCharacter) {
        if (CurrentTarget != null) {
            return;
        }

        Collider[] colliders = Physics.OverlapSphere(aiCharacter.transform.position, _detectionRadius, WorldUtilityManager.Instance.GetCharacterLayers());

        for (int i = 0; i < colliders.Length; i++) {
            CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

            // Check For Null, Self & Dead
            if (targetCharacter == null) { continue; }
            if (targetCharacter == aiCharacter) { continue; }
            if (targetCharacter.IsDead.Value) { continue; }

            // If Character Can Damage Target Make Them Target
            if (WorldUtilityManager.Instance.CanIDamageThisTarget(aiCharacter.CharacterGroup, targetCharacter.CharacterGroup)) {
                Vector3 targetDirection = targetCharacter.transform.position - aiCharacter.transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, aiCharacter.transform.forward);

                // Check If In FOV
                if (viewableAngle > _minDetectionAngle && viewableAngle < _maxDetectionAngle) {
                    // Checks If Character Has Clear Line Of Sight
                    if(Physics.Linecast(
                        aiCharacter.CharacterCombatManager.LockOnTransform.position, 
                        targetCharacter.CharacterCombatManager.LockOnTransform.position, 
                        WorldUtilityManager.Instance.GetEnviroLayers())) {

                        Debug.DrawLine(aiCharacter.CharacterCombatManager.LockOnTransform.position, targetCharacter.CharacterCombatManager.LockOnTransform.position, Color.red);
                    }
                    else {
                        aiCharacter.CharacterCombatManager.SetTarget(targetCharacter);
                    }
                }
            }
        }
    }
}
