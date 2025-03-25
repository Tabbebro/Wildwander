using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleScreenLoadManager : MonoBehaviour
{
    public static TitleScreenLoadManager Instance;

    [SerializeField] List<UI_Character_Save_Slot> SaveSlotList = new();

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    private void OnEnable() {
        CheckForSaveFiles();
    }

    public void TryToSelectFirstSlot() {
        foreach (UI_Character_Save_Slot slot in SaveSlotList) {
            // Checks if slot is active in hierarchy
            if (slot.gameObject.activeInHierarchy) {

                // Gets First Character Slot, Sets It As Current Slot & Selects It
                slot.ThisSelectable.Select();
                TitleScreenManager.Instance.CurrentSelectedSlot = slot.CharacterSlot;
                return;
            }
        }
        TitleScreenManager.Instance.CurrentSelectedSlot = CharacterSlot.NO_Slot;
        TitleScreenManager.Instance.CloseLoadGameMenu();
    }

    void CheckForSaveFiles() {
        // Check For Save Slots
        foreach (UI_Character_Save_Slot saveSlot in SaveSlotList) {
            saveSlot.LoadSaveSlots();
        }
        // Select The First Active Slot
        TryToSelectFirstSlot();
    }
}
