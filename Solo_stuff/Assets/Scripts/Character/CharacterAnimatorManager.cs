using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager _character;

    int _horizontal;
    int _vertical;

    [Header("Damage Animations")]
    public string LastDamageAnimationPlayed;

    [SerializeField] string Hit_Front_Medium_01 = "Hit_Front_Medium_01";
    [SerializeField] string Hit_Front_Medium_02 = "Hit_Front_Medium_02";

    [SerializeField] string Hit_Back_Medium_01 = "Hit_Back_Medium_01";
    [SerializeField] string Hit_Back_Medium_02 = "Hit_Back_Medium_02";

    [SerializeField] string Hit_Left_Medium_01 = "Hit_Left_Medium_01";
    [SerializeField] string Hit_Left_Medium_02 = "Hit_Left_Medium_02";

    [SerializeField] string Hit_Right_Medium_01 = "Hit_Right_Medium_01";
    [SerializeField] string Hit_Right_Medium_02 = "Hit_Right_Medium_02";

    public List<string> front_Medium_Damage = new();
    public List<string> back_Medium_Damage = new();
    public List<string> left_Medium_Damage = new();
    public List<string> right_Medium_Damage = new();

    protected virtual void Awake() {
        _character = GetComponent<CharacterManager>();

        _vertical = Animator.StringToHash("Vertical");
        _horizontal = Animator.StringToHash("Horizontal");
    }

    protected virtual void Start() {
        front_Medium_Damage.Add(Hit_Front_Medium_01);
        front_Medium_Damage.Add(Hit_Front_Medium_02);

        back_Medium_Damage.Add(Hit_Back_Medium_01);
        back_Medium_Damage.Add(Hit_Back_Medium_02);

        left_Medium_Damage.Add(Hit_Left_Medium_01);
        left_Medium_Damage.Add(Hit_Left_Medium_02);

        right_Medium_Damage.Add(Hit_Right_Medium_01);
        right_Medium_Damage.Add(Hit_Right_Medium_02);
    }

    public string GetRandomAnimationFromList(List<string> animationList) {
        List<string> finalList = new();

        foreach (string animation in animationList) {
            finalList.Add(animation);
        }

        // Check If Already Played
        finalList.Remove(LastDamageAnimationPlayed);

        // Checks & Removes Nulls
        for (int i = finalList.Count - 1; i > -1 ; i--) {
            if (finalList[i] == null) {
                finalList.RemoveAt(i);
            }
        }

        int randomValue = Random.Range(0, finalList.Count);

        return finalList[randomValue];
    }

    public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue, bool isSprinting) {

        float snappedHorizontal;
        float snappedVertical;

        // Clamp Horizontal Values
        if (horizontalValue > 0 && horizontalValue <= 0.5f) {
            snappedHorizontal = 0.5f;
        }
        else if (horizontalValue > 0.5f && horizontalValue <= 1) {
            snappedHorizontal = 1f;
        }
        else if (horizontalValue < 0 && horizontalValue >= -0.5f) {
            snappedHorizontal = -0.5f;
        }
        else if (horizontalValue < -0.5f && horizontalValue >= -1) {
            snappedHorizontal = -1f;
        }
        else {
            snappedHorizontal = 0f;
        }
        // Clamp Vertical Values
        if (verticalValue > 0 && verticalValue <= 0.5f) {
            snappedVertical = 0.5f;
        }
        else if (verticalValue > 0.5f && verticalValue <= 1) {
            snappedVertical = 1f;
        }
        else if (verticalValue < 0 && verticalValue >= -0.5f) {
            snappedVertical = -0.5f;
        }
        else if (verticalValue < -0.5f && verticalValue >= -1) {
            snappedVertical = -1f;
        }
        else {
            snappedVertical = 0f;
        }

        if (isSprinting) {
            snappedVertical = 2;
        }

        _character.Animator.SetFloat(_horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
        _character.Animator.SetFloat(_vertical, snappedVertical, 0.1f, Time.deltaTime);

    }

    public virtual void PlayTargetActionAnimation(
        string targetAnimation, 
        bool isPerformingAnimation, 
        bool applyRootMotion = true, 
        bool canRotate = false, 
        bool canMove = false
        ) {

        _character.ApplyRootMotion = applyRootMotion;
        _character.Animator.CrossFade(targetAnimation, 0.2f);

        _character.IsPerformingAction = isPerformingAnimation;
        _character.CanRotate = canRotate;
        _character.CanMove = canMove;

        _character.CharacterNetworkManager.NotifyServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
    }

    public virtual void PlayTargetAttackActionAnimation(
        AttackType attackType,
        string targetAnimation,
        bool isPerformingAnimation,
        bool applyRootMotion = true,
        bool canRotate = false,
        bool canMove = false
    ) {

        // TODO: Combo Check
        // TODO: Keep Track Of Attack Type
        // TODO: Update Animations
        // TODO: Parry?

        _character.CharacterCombatManager.CurrentAttackType = attackType;

        _character.ApplyRootMotion = applyRootMotion;
        _character.Animator.CrossFade(targetAnimation, 0.2f);

        _character.IsPerformingAction = isPerformingAnimation;
        _character.CanRotate = canRotate;
        _character.CanMove = canMove;

        _character.CharacterNetworkManager.NotifyServerOfAttackActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
    }
}
