using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleScreenInputReader : MonoBehaviour
{
    public static TitleScreenInputReader Instance;

    public Inputs Inputs;

    TitleScreenManager _titleScreenManager;

    [Header("Selection")]
    [ReadOnly][SerializeField] GameObject _currentSelectable;
    [ReadOnly][SerializeField] GameObject _previousSelectable;
    public bool SelectionDisabled;

    [Header("Menus")]
    [SerializeField] GameObject _titleMainMenu;
    [SerializeField] GameObject _titleLoadCharacterMenu;

    [Header("Pop Ups")]
    public GameObject DeleteCharacterPopUp;
    [SerializeField] GameObject _noCharactersAvailablePopUp;

    [Header("Buttons")]
    bool _northButton = false;
    bool _southButton = false;
    bool _eastButton = false;
    bool _westButton = false;

    bool _navigation = false;

    private void Awake() {

        if(Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }

        _titleScreenManager = GetComponent<TitleScreenManager>();
    }

    private void OnEnable() {
        if (Inputs == null) {
            Inputs = new();
            Inputs.UI.ButtonNorth.performed += i => _northButton = true;
            Inputs.UI.ButtonEast.performed += i => _eastButton = true;
            Inputs.UI.ButtonSouth.performed += i => _southButton = true;
            Inputs.UI.ButtonWest.performed += i => _westButton = true;
            Inputs.UI.Navigation.performed += i => _navigation = true;
        }

        Inputs.UI.Enable();
    }

    private void OnDisable() {

        Inputs.UI.Disable();
    }

    private void Update() {
        CheckSelectionLoss();
        CharacterLoadInputs();
        DeleteCharacterPopUpInputs();
        NoCharactersAvailablePopUpInputs();
    }

    #region Inputs

    void CharacterLoadInputs() {
        if (!_titleLoadCharacterMenu.activeInHierarchy || DeleteCharacterPopUp.activeInHierarchy) { return; }
        print("Load");

        if (_northButton) {
            _northButton = false;
            print("North");
        }
        if (_eastButton) { 
            _eastButton = false;
            print("East");
            _titleScreenManager.CloseLoadGameMenu();
        
        }
        if (_southButton) {
            _southButton = false;
            print("South");

        }
        if (_westButton) { 
            _westButton = false;
            print("West");
            _titleScreenManager.AttemptToDeleteCharacterSlot();
        }

    }

    void DeleteCharacterPopUpInputs() {
        if (!DeleteCharacterPopUp.activeInHierarchy) { return; }

        if (_northButton) {
            _northButton = false;
        }
        if (_eastButton) {
            _eastButton = false;
            _titleScreenManager.CloseDeleteCharacterPopUp();
        }
        if (_southButton) {
            _southButton = false;

        }
        if (_westButton) {
            _westButton = false;
            
        }
    }

    void NoCharactersAvailablePopUpInputs() {
        if (!_noCharactersAvailablePopUp.activeInHierarchy) { return; }

        if (_northButton) {
            _northButton = false;
        }
        if (_eastButton) {
            _eastButton = false;
            _titleScreenManager.CloseNoFreeCharactersMessage();
        }
        if (_southButton) {
            _southButton = false;

        }
        if (_westButton) {
            _westButton = false;

        }
    }

    #endregion

    #region Selection Loss
    void CheckSelectionLoss() {

        GameObject current = EventSystem.current.currentSelectedGameObject;
        if (current != _currentSelectable) {
            if (_currentSelectable != null) {
                _previousSelectable = _currentSelectable;
            }
            _currentSelectable = current;
        }

        if (_currentSelectable == null && !SelectionDisabled && _navigation) {
            _navigation = false;
            EventSystem.current.SetSelectedGameObject(_previousSelectable);
        }

    }
    #endregion
}
