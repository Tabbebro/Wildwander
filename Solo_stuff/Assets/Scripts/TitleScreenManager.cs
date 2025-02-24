using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    public void StartNewGame() {
        StartCoroutine(WorldSaveGameManager.Instance.LoadNewGame());
    }
}
