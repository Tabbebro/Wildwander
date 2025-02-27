using UnityEngine;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    [SerializeField] GameObject _playerPrefab;
    [SerializeField] GameObject _firstCheck;
    [SerializeField] GameObject _mainMenu;

    [Header("Selectables")]
    [SerializeField] Selectable _mainMenuFirstSelectable;

    public void PressStart() {
        _firstCheck.SetActive(false);
        _mainMenu.SetActive(true);
        _mainMenuFirstSelectable.Select();
        Instantiate(_playerPrefab);
    }
    public void StartNewGame() {
        StartCoroutine(WorldSaveGameManager.Instance.LoadNewGame());
    }
}
