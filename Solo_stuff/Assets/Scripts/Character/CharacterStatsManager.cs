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

    protected virtual void Start() {

    }

    public int CalculateHealthBasedOnLevel(int vitality) {
        float health = 0;


        // TODO: Get a better equation for health
        health = vitality * 15;
        return Mathf.RoundToInt(health);
    }

    #region Stamina
    public int CalculateStaminaBasedOnLevel(int endurance) {
        float stamina = 0;


        // TODO: Get a better equation for stamina
        stamina = endurance * 10;

        return Mathf.RoundToInt(stamina);
    }

    public virtual void RegenerateStamina() {

        if (!_character.IsOwner) { return; }

        // Don't regenerate when sprinting or performing action
        if (_character.CharacterNetworkManager.IsSprinting.Value || _character.IsPerformingAction) {
            return;
        }

        _staminaRegenerationTimer += Time.deltaTime;

        if (_staminaRegenerationTimer >= _staminaRegenerationDelay && _character.CharacterNetworkManager.CurrentStamina.Value < _character.CharacterNetworkManager.MaxStamina.Value) {
            _character.CharacterNetworkManager.CurrentStamina.Value += _staminaRegenerationAmount * Time.deltaTime;
        }
    }

    public virtual void ResetStaminaRegenTimer(float previousStaminaValue, float currentStaminaAmount) {
        if (currentStaminaAmount < previousStaminaValue) {
            _staminaRegenerationTimer = 0;
        }
    }
    #endregion
}