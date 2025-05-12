using UnityEngine;

public class Enums : MonoBehaviour
{
    
}

public enum CharacterSlot {
    CharacterSlot_01,
    CharacterSlot_02,
    CharacterSlot_03,
    CharacterSlot_04,
    CharacterSlot_05,
    CharacterSlot_06,
    CharacterSlot_07,
    CharacterSlot_08,
    CharacterSlot_09,
    CharacterSlot_10,
    NO_Slot
}

public enum CharacterGroup {
    Player,
    FriendlyPhantom,
    HostilePhantom,
    EnemyGroup01
}

public enum WeaponModelSlot {
    RightHand, 
    LeftHand,
    Back
}

public enum AttackType {
    LightAttack01,
    LightAttack02,
    HeavyAttack01,
    HeavyAttack02,
    HeavyAttackHold01,
    HeavyAttackHold02,
    RunningAttack01,
    RollingAttack01,
    BackstepAttack01
}

public enum SurfaceType {
    Untagged,
    Dirt,
    Concrete,
    Gravel,
    Stone
}
