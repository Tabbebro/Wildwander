using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class TitleScreenManager : MonoBehaviour
{
    [SerializeField] GameObject _firstCheck;
    [SerializeField] GameObject _mainMenu;

    [Header("Selectables")]
    [SerializeField] Selectable _mainMenuFirstSelectable;

    public void PressStart() {
        _firstCheck.SetActive(false);
        _mainMenu.SetActive(true);
        _mainMenuFirstSelectable.Select();
    }
    public void StartNewGame() {
        WorldSaveGameManager.Instance.NewGame();
        StartCoroutine(WorldSaveGameManager.Instance.LoadWorldScene());
    }

    public void StartNetworkAsHost() {
        NetworkManager.Singleton.StartHost();
    }
}
