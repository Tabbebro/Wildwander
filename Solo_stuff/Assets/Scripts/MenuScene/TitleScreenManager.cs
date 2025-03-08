using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class TitleScreenManager : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] GameObject _firstCheck;
    [SerializeField] GameObject _titleMainMenu;
    [SerializeField] GameObject _titleLoadGameMenu;

    [Header("Buttons")]
    [SerializeField] Selectable _mainMenuStartGameButton;
    [SerializeField] Selectable _mainMenuLoadGameButton;
    [SerializeField] Selectable _loadMenuReturnButton;

    public void PressStart() {
        _firstCheck.SetActive(false);
        _titleMainMenu.SetActive(true);
        _mainMenuStartGameButton.Select();
    }
    public void StartNewGame() {
        WorldSaveGameManager.Instance.NewGame();
        StartCoroutine(WorldSaveGameManager.Instance.LoadWorldScene());
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
}
