using TMPro;
using UnityEngine;

public class UI_CharacterHpBar : UI_StatBar
{
    CharacterManager _character;
    AICharacterManager _aiCharacter;
    PlayerManager _playerCharacter;

    [Header("Character HP Bar")]
    [SerializeField] bool _displayCharacterNameOnDamage = false;
    [SerializeField] float _defaultTimeBeforeBarHides = 3f;
    [SerializeField] float _hideTimer = 0;
    [SerializeField] int _currentDamageTaken = 0;
    [SerializeField] TextMeshProUGUI _characterName;
    [SerializeField] TextMeshProUGUI _characterDamage;
    [HideInInspector] public int OldHealthValue = 0;

    protected override void Awake() {
        base.Awake();

        _character = GetComponentInParent<CharacterManager>();
        if (_character != null) {
            _aiCharacter = _character as AICharacterManager;
        }
        if (_character != null) {
            _playerCharacter = _character as PlayerManager;
        }
    }

    protected override void Start() {
        base.Start();

        gameObject.SetActive(false);
    }

    public override void SetStat(int newValue) {
        if (_displayCharacterNameOnDamage) {
            _characterName.enabled = true;

            // Get Name If AI
            if (_aiCharacter != null) {
                
                _characterName.text = _aiCharacter.CharacterName;
            }
            // Get Name If Player
            if (_playerCharacter != null) {

                _characterName.text = _playerCharacter.PlayerNetworkManager.CharacterName.Value.ToString();
            }
        }

        _slider.maxValue = _character.CharacterNetworkManager.MaxHealth.Value;

        ResetSecondaryBarTimer(_slider.value, newValue);

        _currentDamageTaken = Mathf.RoundToInt(_currentDamageTaken + (OldHealthValue - newValue));

        if (_currentDamageTaken < 0) {
            _currentDamageTaken = Mathf.Abs(_currentDamageTaken);
            _characterDamage.text = "+ " + _currentDamageTaken.ToString();
        }
        else {
            _characterDamage.text = "- " + _currentDamageTaken.ToString();
        }

        _slider.value = newValue;

        if (_character.CharacterNetworkManager.CurrentHealth.Value != _character.CharacterNetworkManager.MaxHealth.Value) {
            _hideTimer = _defaultTimeBeforeBarHides;
            gameObject.SetActive(true);
        }
    }

    protected override void Update() {
        base.Update();
        transform.LookAt(transform.position + Camera.main.transform.forward);

        if (_hideTimer > 0) {
            _hideTimer -= Time.deltaTime;
        }
        else {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable() {
        _currentDamageTaken = 0;
    }
}
