using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleScreenInputReader : MonoBehaviour
{
    Inputs _inputs;

    [Header("Selection")]
    [ReadOnly][SerializeField] GameObject _currentSelectable;
    [ReadOnly][SerializeField] GameObject _previousSelectable;
    public bool SelectionDisabled;

    [Header("Menus")]
    [SerializeField] GameObject _titleMainMenu;
    [SerializeField] GameObject _titleLoadCharacterMenu;

    [Header("Pop Ups")]
    [SerializeField] GameObject _deleteCharacterPopUp;
    [SerializeField] GameObject _noCharactersAvailablePopUp;

    [Header("Buttons")]
    bool _northButton = false;
    bool _southButton = false;
    bool _eastButton = false;
    bool _westButton = false;

    bool _navigation = false;

    private void OnEnable() {
        if (_inputs == null) {
            _inputs = new();
            _inputs.UI.ButtonNorth.performed += i => _northButton = true;
            _inputs.UI.ButtonEast.performed += i => _eastButton= true;
            _inputs.UI.ButtonSouth.performed += i => _southButton = true;
            _inputs.UI.ButtonWest.performed += i => _westButton = true;
            _inputs.UI.Navigation.performed += i => _navigation = true;
        }

        _inputs.UI.Enable();
    }

    private void OnDisable() {

        _inputs.UI.Disable();
    }

    private void Update() {
        CheckSelectionLoss();
        CharacterLoadInputs();
        DeleteCharacterPopUpInputs();
        NoCharactersAvailablePopUpInputs();
    }

    #region Inputs

    void CharacterLoadInputs() {
        if (!_titleLoadCharacterMenu.activeInHierarchy || _deleteCharacterPopUp.activeInHierarchy) { return; }


        if (_northButton) {
            _northButton = false;
        }
        if (_eastButton) { 
            _eastButton = false;
            TitleScreenManager.Instance.CloseLoadGameMenu();
        
        }
        if (_southButton) {
            _southButton = false;

        }
        if (_westButton) { 
            _westButton = false;
            TitleScreenManager.Instance.AttemptToDeleteCharacterSlot();
        }

    }

    void DeleteCharacterPopUpInputs() {
        if (!_deleteCharacterPopUp.activeInHierarchy) { return; }
        print("In Delete character");

        if (_northButton) {
            _northButton = false;
        }
        if (_eastButton) {
            _eastButton = false;
            TitleScreenManager.Instance.CloseDeleteCharacterPopUp();
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
        print("In no characters available");

        if (_northButton) {
            _northButton = false;
        }
        if (_eastButton) {
            _eastButton = false;
            TitleScreenManager.Instance.CloseNoFreeCharactersMessage();
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
            print("hehe");
            _navigation = false;
            EventSystem.current.SetSelectedGameObject(_previousSelectable);
        }

    }
    #endregion
}
