using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class TitleScreenManager : MonoBehaviour {
    public static TitleScreenManager Instance;

    [Header("Menus")]
    [SerializeField] GameObject _firstCheck;
    [SerializeField] GameObject _titleMainMenu;
    [SerializeField] GameObject _titleLoadGameMenu;

    [Header("Buttons")]
    [SerializeField] Selectable _mainMenuStartGameButton;
    [SerializeField] Selectable _mainMenuLoadGameButton;
    [SerializeField] Selectable _loadMenuReturnButton;
    [SerializeField] Selectable _noCharacterSlotsPopUpButton;
    [SerializeField] Selectable _deleteCharacterPopUpConfirmButton;


    [Header("Pop Ups")]
    [SerializeField] GameObject _noCharacterSlotsPopUp;
    [SerializeField] GameObject _deleteCharacterSlotPopUp;

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

    public void PressStart() {
        _firstCheck.SetActive(false);
        _titleMainMenu.SetActive(true);
        _mainMenuStartGameButton.Select();
    }
    public void StartNewGame() {
        WorldSaveGameManager.Instance.AttemptToCreateNewGame();
    }
    public void StartNetworkAsHost() {
        NetworkManager.Singleton.StartHost();
    }
    public void OpenLoadGameMenu() {

        _titleMainMenu.SetActive(false);
        _titleLoadGameMenu.SetActive(true);
        _loadMenuReturnButton.Select();
    }
    public void CloseLoadGameMenu() {

        _titleLoadGameMenu.SetActive(false);
        _titleMainMenu.SetActive(true);
        _mainMenuLoadGameButton.Select();
    }
    public void DisplayNoFreeCharactersMessage() {
        _noCharacterSlotsPopUp.SetActive(true);
        _noCharacterSlotsPopUpButton.Select();
    }
    public void CloseNoFreeCharactersMessage() {
        _noCharacterSlotsPopUp.SetActive(false);
        _mainMenuStartGameButton.Select();
    }

    // Character Slots
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
        _loadMenuReturnButton.Select();
    }
    public void CloseDeleteCharacterPopUp() {
        _deleteCharacterSlotPopUp.SetActive(false);
        _loadMenuReturnButton.Select();
    }
}
