using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    CharacterManager _character;

    [Header("Stamina Regeneration")]
    [SerializeField] int _staminaRegenerationAmount = 2;
    float _staminaRegenerationTimer = 0;
    float _staminaTickTimer = 0;
    [SerializeField] float _staminaRegenerationDelay = 2;

    protected virtual void Awake() {
        _character = GetComponent<CharacterManager>();
    }

    public int CalculateStaminaBasedOnLevel(int endurance) {
        float stamina = 0;


        // TODO: Get a better equation for stamina
        stamina = endurance * 10;

        return Mathf.RoundToInt(stamina);
    }

    public virtual void RegenerateStamina() {

        // Don't regenerate when sprinting or performing action
        if (_character._characterNetworkManager.IsSprinting.Value || _character.isPerformingAction) {
            return;
        }

        _staminaRegenerationTimer += Time.deltaTime;

        if (_staminaRegenerationTimer >= _staminaRegenerationDelay && _character.CurrentStamina < _character.MaxStamina) {

            _staminaTickTimer += Time.deltaTime;
            if (_staminaTickTimer >= 0.1f) {
                _staminaTickTimer = 0;
                _character.SetCurrentStamina(_character.CurrentStamina + _staminaRegenerationAmount);
            }
        }
    }

    public virtual void ResetStaminaRegenTimer(float previousStaminaValue, float currentStaminaAmount) {
        if (previousStaminaValue < currentStaminaAmount) {
            _staminaRegenerationTimer = 0;
        }
    }
}