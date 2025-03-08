using System.IO;
using TMPro;
using UnityEditor.Overlays;
using UnityEngine;

public class UI_Character_Save_Slot : MonoBehaviour
{
    SaveFileWriter _saveFileWriter;

    [Header("Save Slot")]
    public CharacterSlot CharacterSlot;

    [Header("Character Info")]
    public TextMeshProUGUI CharacterName;
    public TextMeshProUGUI TimePlayed;

    private void OnEnable() {
        LoadSaveSlots();
    }

    void LoadSaveSlots() {
        _saveFileWriter = new();
        _saveFileWriter.SaveDataPath = Application.persistentDataPath;

        switch (CharacterSlot) {
            // Save Slot 01
            case CharacterSlot.CharacterSlot_01:
                CharacterSlotLoading(CharacterSlot, ref _saveFileWriter, WorldSaveGameManager.Instance.CharacterSlot01);
                break;
            // Save Slot 02
            case CharacterSlot.CharacterSlot_02:
                CharacterSlotLoading(CharacterSlot, ref _saveFileWriter, WorldSaveGameManager.Instance.CharacterSlot02);
                break;
            // Save Slot 03
            case CharacterSlot.CharacterSlot_03:
                CharacterSlotLoading(CharacterSlot, ref _saveFileWriter, WorldSaveGameManager.Instance.CharacterSlot03);
                break;
            // Save Slot 04
            case CharacterSlot.CharacterSlot_04:
                CharacterSlotLoading(CharacterSlot, ref _saveFileWriter, WorldSaveGameManager.Instance.CharacterSlot04);
                break;
            // Save Slot 05
            case CharacterSlot.CharacterSlot_05:
                CharacterSlotLoading(CharacterSlot, ref _saveFileWriter, WorldSaveGameManager.Instance.CharacterSlot05);
                break;
            // Save Slot 06
            case CharacterSlot.CharacterSlot_06:
                CharacterSlotLoading(CharacterSlot, ref _saveFileWriter, WorldSaveGameManager.Instance.CharacterSlot06);
                break;
            // Save Slot 07
            case CharacterSlot.CharacterSlot_07:
                CharacterSlotLoading(CharacterSlot, ref _saveFileWriter, WorldSaveGameManager.Instance.CharacterSlot07);
                break;
            // Save Slot 08
            case CharacterSlot.CharacterSlot_08:
                CharacterSlotLoading(CharacterSlot, ref _saveFileWriter, WorldSaveGameManager.Instance.CharacterSlot08);
                break;
            // Save Slot 09
            case CharacterSlot.CharacterSlot_09:
                CharacterSlotLoading(CharacterSlot, ref _saveFileWriter, WorldSaveGameManager.Instance.CharacterSlot09);
                break;
            // Save Slot 10
            case CharacterSlot.CharacterSlot_10:
                CharacterSlotLoading(CharacterSlot, ref _saveFileWriter, WorldSaveGameManager.Instance.CharacterSlot10);
                break;
            default:
                break;
        }
    }

    private void CharacterSlotLoading(CharacterSlot characterSlot, ref SaveFileWriter writer, CharacterSaveData saveData) {
        writer.SaveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
        // If File Exists Load Character Data
        if (writer.CheckToSeeIfFileExists()) {
            print("File does exist" + "\n" + "save file name: " + Path.Combine(writer.SaveDataPath, writer.SaveFileName));
            CharacterName.text = saveData.CharacterName;
        }
        // Else Hide Game Object
        else {
            print("File doesn't exist");
            gameObject.SetActive(false);
        }
    }

    public void LoadGameFromCharacterSlot() {
        WorldSaveGameManager.Instance.CurrentCharacterSlotUsed = CharacterSlot;
        WorldSaveGameManager.Instance.LoadGame();
    }
}
