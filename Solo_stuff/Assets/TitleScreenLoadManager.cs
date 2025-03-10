using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenLoadManager : MonoBehaviour
{
    public static TitleScreenLoadManager Instance;

    [SerializeField] List<Selectable> SaveSlotSelectableList = new();

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    private void OnEnable() {
        TryToSelectFirstSlot();   
    }

    public void TryToSelectFirstSlot() {
        foreach (Selectable slot in SaveSlotSelectableList) {
            // Checks if slot is active in hierarchy
            if (slot.gameObject.activeInHierarchy) {
                // selects first active slot & returns
                slot.Select();
                return;
            }
        }
        TitleScreenManager.Instance.CloseLoadGameMenu();
    }
}
