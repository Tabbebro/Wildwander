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

    [Header("Pop Ups")]
    [SerializeField] GameObject _noCharacterSlotsPopUp;
    [SerializeField] Selectable _noCharacterSlotsPopUpButton;

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
}
