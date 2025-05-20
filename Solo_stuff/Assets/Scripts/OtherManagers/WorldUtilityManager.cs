using UnityEngine;

public class WorldUtilityManager : MonoBehaviour
{
    public static WorldUtilityManager Instance;

    [Header("Layers")]
    [SerializeField] LayerMask _characterLayers;
    [SerializeField] LayerMask _enviroLayers;



    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    public LayerMask GetCharacterLayers() {
        return _characterLayers;
    }

    public LayerMask GetEnviroLayers() {
        return _enviroLayers;
    }

    public bool CanIDamageThisTarget(CharacterGroup attackingCharacter, CharacterGroup targetCharacter) {
        // TODO: Add more groups
        switch (attackingCharacter) {

            case CharacterGroup.Player:
                switch (targetCharacter) {
                    case CharacterGroup.Player:             return false;
                    case CharacterGroup.FriendlyPhantom:    return false;
                    case CharacterGroup.HostilePhantom:     return true;
                    case CharacterGroup.EnemyGroup01:       return true;
                    default:                                break;
                }
            break;

            case CharacterGroup.FriendlyPhantom:
                switch (targetCharacter) {
                    case CharacterGroup.Player:             return false;
                    case CharacterGroup.FriendlyPhantom:    return false;
                    case CharacterGroup.HostilePhantom:     return true;
                    case CharacterGroup.EnemyGroup01:       return true;
                    default:                                break;
                }
            break;

            case CharacterGroup.HostilePhantom:
                switch (targetCharacter) {
                    case CharacterGroup.Player:             return true;
                    case CharacterGroup.FriendlyPhantom:    return true;
                    case CharacterGroup.HostilePhantom:     return false;
                    case CharacterGroup.EnemyGroup01:       return false;
                    default:                                break;
                }
            break;

            case CharacterGroup.EnemyGroup01:
                switch (targetCharacter) {
                    case CharacterGroup.Player:             return true;
                    case CharacterGroup.FriendlyPhantom:    return true;
                    case CharacterGroup.HostilePhantom:     return false;
                    case CharacterGroup.EnemyGroup01:       return false;
                    default:                                break;
                }
            break;

            default:    break;
        }

        return false;
    }

    public float GetAngleOfTarget(Transform characterTransform, Vector3 targetDirection) {
        targetDirection.y = 0;
        float viewableAngle = Vector3.Angle(characterTransform.forward, targetDirection);
        Vector3 cross = Vector3.Cross(characterTransform.forward, targetDirection);

        if (cross.y < 0) {
            viewableAngle = -viewableAngle;
        }

        return viewableAngle;
    }

    public DamageIntensity GetDamageIntensityBasedOnPoiseDamage(float poiseDamage) {
        
        // Small Damage (Throwing Daggers & Rocks)
        DamageIntensity damageIntensity = DamageIntensity.Ping;

        // Light Weapons (Daggers)
        if (poiseDamage >= 10) {
            damageIntensity = DamageIntensity.Light;
        }

        // Standard Weapons (Swords, Maces & Axes)
        if (poiseDamage >= 30) {
            damageIntensity = DamageIntensity.Medium;
        }

        // Big Weapons (Greatswords & such)
        if (poiseDamage >= 70) {
            damageIntensity = DamageIntensity.Heavy;
        }

        // Colossal Weapons (Ultra Greatswords & Some Big Bosses)
        if (poiseDamage >= 120) {
            damageIntensity = DamageIntensity.Colossal;
        }


        return damageIntensity;
    }
}
