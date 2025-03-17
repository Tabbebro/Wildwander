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

                // Gets First Character Slot, Sets It As Current Slot & Selects It
                TitleScreenManager.Instance.CurrentSelectedSlot = slot.GetComponent<UI_Character_Save_Slot>().CharacterSlot;
                slot.Select();
                return;
            }
        }
        TitleScreenManager.Instance.CurrentSelectedSlot = CharacterSlot.NO_Slot;
        TitleScreenManager.Instance.CloseLoadGameMenu();
    }
}
