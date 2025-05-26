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
    LeftHandWeapon,
    LeftHandShield,
    Back
}

public enum WeaponModelType {
    Weapon,
    Shield
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

public enum DamageIntensity {
    Ping,
    Light,
    Medium,
    Heavy,
    Colossal
}

public enum SurfaceType {
    Untagged,
    Dirt,
    Concrete,
    Gravel,
    Stone
}

public enum ControlScheme {
    Keyboard,
    Controller
}

public enum ControllerType {
    Playstation,
    Xbox
}
