using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class TitleScreenManager : MonoBehaviour {
    public static TitleScreenManager Instance;

    [Header("Menus")]
    [SerializeField] GameObject _firstCheck;
    [SerializeField] GameObject _titleMainMenu;
    [SerializeField] GameObject _titleLoadGameMenu;

    [Header("Back Grounds")]
    [SerializeField] GameObject _firstCheckBG;
    [SerializeField] GameObject _titleMainMenuBG;

    [Header("Buttons")]
    [SerializeField] Selectable _mainMenuStartGameButton;
    [SerializeField] Selectable _mainMenuLoadGameButton;
    [SerializeField] Selectable _loadMenuReturnButton;
    [SerializeField] Selectable _noCharacterSlotsPopUpButton;
    [SerializeField] Selectable _deleteCharacterPopUpConfirmButton;


    [Header("Pop Ups")]
    [SerializeField] GameObject _noCharacterSlotsPopUp;
    [SerializeField] GameObject _deleteCharacterSlotPopUp;

    [Header("Texts")]
    [SerializeField] TextMeshProUGUI _versionText;

    [Header("Save Slots")]
    public CharacterSlot CurrentSelectedSlot = CharacterSlot.NO_Slot;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        _versionText.text = "Version: " + Application.version;
    }

    public void PressStart() {
        CloseFirstCheck();
        OpenTitleMainMenu();
    }
    public void StartNewGame() {
        WorldSaveGameManager.Instance.AttemptToCreateNewGame();
    }
    public void StartNetworkAsHost() {
        NetworkManager.Singleton.StartHost();
    }
    public void DisplayNoFreeCharactersMessage() {
        _noCharacterSlotsPopUp.SetActive(true);
        _noCharacterSlotsPopUpButton.Select();
    }
    public void CloseNoFreeCharactersMessage() {
        _noCharacterSlotsPopUp.SetActive(false);
        _mainMenuStartGameButton.Select();
    }

    #region Open/Close Different Menus
    void OpenFirstCheck() {
        _firstCheck.SetActive(true);
        _firstCheckBG.SetActive(true);
    }

    void CloseFirstCheck() {
        _firstCheck.SetActive(false);
        _firstCheckBG.SetActive(false);
    }

    void OpenTitleMainMenu() {
        _titleMainMenu.SetActive(true);
        _titleMainMenuBG.SetActive(true);
        _mainMenuStartGameButton.Select();
    }

    void CloseTitleMainMenu() {
        _titleMainMenu.SetActive(false);
        _titleMainMenuBG.SetActive(false);
    }
    public void OpenLoadGameMenu() {

        _titleMainMenu.SetActive(false);
        _titleLoadGameMenu.SetActive(true);
    }
    public void CloseLoadGameMenu() {

        _titleLoadGameMenu.SetActive(false);
        _titleMainMenu.SetActive(true);
        _mainMenuLoadGameButton.Select();
    }
    #endregion
    #region Character Slots
    public void SelectCharacterSlot(CharacterSlot characterSlot) {
        CurrentSelectedSlot = characterSlot;
    }
    public void SelectNoSlot() {
        CurrentSelectedSlot = CharacterSlot.NO_Slot;
    }
    public void AttemptToDeleteCharacterSlot() {
        // If current Selected Slot Is A Valid Slot
        if (CurrentSelectedSlot != CharacterSlot.NO_Slot) {

            // Open Pop Up
            _deleteCharacterSlotPopUp.SetActive(true);

            // Select First Button
            _deleteCharacterPopUpConfirmButton.Select();
        }
    }
    public void DeleteCharacterSlot() {
        // Deactivate Pop Up
        _deleteCharacterSlotPopUp.SetActive(false);

        // Delete Save
        WorldSaveGameManager.Instance.DeleteGame(CurrentSelectedSlot);

        // Refresh Menu
        _titleLoadGameMenu.SetActive(false);
        _titleLoadGameMenu.SetActive(true);

        // Select Return Button
        //_loadMenuReturnButton.Select();
        TitleScreenLoadManager.Instance.TryToSelectFirstSlot();
    }
    public void CloseDeleteCharacterPopUp() {
        _deleteCharacterSlotPopUp.SetActive(false);
        _loadMenuReturnButton.Select();
        TitleScreenLoadManager.Instance.TryToSelectFirstSlot();
    }
    #endregion
}
