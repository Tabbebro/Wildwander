using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSFXManager : CharacterSFXManager
{
    bool _correctScene; // Disables footstep sounds when in main menu

    protected virtual void Start() {
        SceneManager.activeSceneChanged += OnSceneChange;
    }

    private void OnDestroy() {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    private void OnSceneChange(Scene oldScene, Scene newScene) {
        if (newScene.buildIndex == WorldSaveGameManager.Instance.GetWorldSceneIndex()) {
            _correctScene = true;
        }
        else {
            _correctScene = false;
        }
    }
    protected override void Footstep() {
        if (!_correctScene) { return; }
        base.Footstep();
    }
}
