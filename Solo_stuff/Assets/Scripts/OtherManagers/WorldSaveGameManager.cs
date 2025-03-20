using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSaveGameManager : MonoBehaviour
{
    public static WorldSaveGameManager Instance;

    public PlayerManager Player;

    [Header("Save/Load")]
    [SerializeField] bool _saveGame;
    [SerializeField] bool _loadGame;

    [Header("World Scene Index")]
    [SerializeField] int worldSceneIndex = 1;

    [Header("Save Data Writer")]
    SaveFileWriter saveFileWriter;


    [Header("Current Character Data")]
    public CharacterSlot CurrentCharacterSlotUsed;
    public CharacterSaveData CurrentCharacterData;
    string _fileName;

    [Header("Character Slots")]
    public CharacterSaveData CharacterSlot01;
    public CharacterSaveData CharacterSlot02;
    public CharacterSaveData CharacterSlot03;
    public CharacterSaveData CharacterSlot04;
    public CharacterSaveData CharacterSlot05;
    public CharacterSaveData CharacterSlot06;
    public CharacterSaveData CharacterSlot07;
    public CharacterSaveData CharacterSlot08;
    public CharacterSaveData CharacterSlot09;
    public CharacterSaveData CharacterSlot10;

    private void Awake() {
        if (Instance == null) {
            Instance = this;        
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        DontDestroyOnLoad(gameObject);
        LoadAllCharacterSlots();
    }

    private void Update() {
        if (_saveGame) { 
            _saveGame = false;
            SaveGame();
        }

        if (_loadGame) {
            _loadGame = false;
            LoadGame();
        }
    }

    public string DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot characterSlot) {
        string fileName = "";
        switch (characterSlot) {
            case CharacterSlot.CharacterSlot_01:
                fileName = "Slot_01";
                break;
            case CharacterSlot.CharacterSlot_02:
                fileName = "Slot_02";
                break;
            case CharacterSlot.CharacterSlot_03:
                fileName = "Slot_03";
                break;
            case CharacterSlot.CharacterSlot_04:
                fileName = "Slot_04";
                break;
            case CharacterSlot.CharacterSlot_05:
                fileName = "Slot_05";
                break;
            case CharacterSlot.CharacterSlot_06:
                fileName = "Slot_06";
                break;
            case CharacterSlot.CharacterSlot_07:
                fileName = "Slot_07";
                break;
            case CharacterSlot.CharacterSlot_08:
                fileName = "Slot_08";
                break;
            case CharacterSlot.CharacterSlot_09:
                fileName = "Slot_09";
                break;
            case CharacterSlot.CharacterSlot_10:
                fileName = "Slot_10";
                break;
            default:
                break;
        }
        return fileName;
    }

    public void AttemptToCreateNewGame() {

        saveFileWriter = new();
        saveFileWriter.SaveDataPath = Application.persistentDataPath;

        // Checks if there are available slots to make a new save
        // Slot #1
        if (CheckIfSlotIsUsed(CharacterSlot.CharacterSlot_01, saveFileWriter)) {
            return;
        }
        // Slot # 2
        else if (CheckIfSlotIsUsed(CharacterSlot.CharacterSlot_02, saveFileWriter)) {
            return;
        }
        // Slot # 3
        else if (CheckIfSlotIsUsed(CharacterSlot.CharacterSlot_03, saveFileWriter)) {
            return;
        }
        // Slot # 4
        else if (CheckIfSlotIsUsed(CharacterSlot.CharacterSlot_04, saveFileWriter)) {
            return;
        }
        // Slot # 5
        else if (CheckIfSlotIsUsed(CharacterSlot.CharacterSlot_05, saveFileWriter)) {
            return;
        }
        // Slot # 6
        else if (CheckIfSlotIsUsed(CharacterSlot.CharacterSlot_06, saveFileWriter)) {
            return;
        }
        // Slot # 7
        else if (CheckIfSlotIsUsed(CharacterSlot.CharacterSlot_07, saveFileWriter)) {
            return;
        }
        // Slot # 8
        else if (CheckIfSlotIsUsed(CharacterSlot.CharacterSlot_08, saveFileWriter)) {
            return;
        }
        // Slot # 9
        else if (CheckIfSlotIsUsed(CharacterSlot.CharacterSlot_09, saveFileWriter)) {
            return;
        }
        // Slot # 10
        else if (CheckIfSlotIsUsed(CharacterSlot.CharacterSlot_10, saveFileWriter)) {
            return;
        }

        // If no avalailable slots notify player
        TitleScreenManager.Instance.DisplayNoFreeCharactersMessage();
    }

    void NewGame() {
        // TODO: Remove Later
        Player.PlayerNetworkManager.Vitality.Value = 10;
        Player.PlayerNetworkManager.Endurance.Value = 10;

        SaveGame();

        StartCoroutine(LoadWorldScene());
    }

    bool CheckIfSlotIsUsed(CharacterSlot slot, SaveFileWriter writer) {
        writer.SaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(slot);
        if (!writer.CheckToSeeIfFileExists()) { 
            CurrentCharacterSlotUsed = slot;
            CurrentCharacterData = new();
            NewGame();
            return true; 
        }
        else { return false; }
    }

    public void LoadGame() {
        _fileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CurrentCharacterSlotUsed);

        saveFileWriter = new();
        saveFileWriter.SaveDataPath = Application.persistentDataPath;
        saveFileWriter.SaveFileName = _fileName;

        // Loads Save File
        CurrentCharacterData = saveFileWriter.LoadSaveFile();

        // Loads World Scene
        StartCoroutine(LoadWorldScene());
    }

    public void SaveGame() {
        _fileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CurrentCharacterSlotUsed);

        saveFileWriter = new();
        saveFileWriter.SaveDataPath = Application.persistentDataPath;
        saveFileWriter.SaveFileName = _fileName;

        // Get Values From Player
        Player.SaveDataToCurrentCharacterData(ref CurrentCharacterData);

        // Write Info To Save File
        saveFileWriter.CreateNewCharacterSaveFile(CurrentCharacterData);
    }

    public void DeleteGame(CharacterSlot characterSlot) {
        // Get file to delete
        saveFileWriter = new();
        saveFileWriter.SaveDataPath = Application.persistentDataPath;
        saveFileWriter.SaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
        saveFileWriter.DeleteSaveFile();
    }

    // Load All Character Slots When Starting Game
    private void LoadAllCharacterSlots() {
        saveFileWriter = new();
        saveFileWriter.SaveDataPath = Application.persistentDataPath;

        // Character Slot 01
        saveFileWriter.SaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);
        CharacterSlot01 = saveFileWriter.LoadSaveFile();

        // Character Slot 02
        saveFileWriter.SaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);
        CharacterSlot02 = saveFileWriter.LoadSaveFile();

        // Character Slot 03
        saveFileWriter.SaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);
        CharacterSlot03 = saveFileWriter.LoadSaveFile();

        // Character Slot 04
        saveFileWriter.SaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);
        CharacterSlot04 = saveFileWriter.LoadSaveFile();

        // Character Slot 05
        saveFileWriter.SaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);
        CharacterSlot05 = saveFileWriter.LoadSaveFile();

        // Character Slot 06
        saveFileWriter.SaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_06);
        CharacterSlot06 = saveFileWriter.LoadSaveFile();

        // Character Slot 07
        saveFileWriter.SaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_07);
        CharacterSlot07 = saveFileWriter.LoadSaveFile();

        // Character Slot 08
        saveFileWriter.SaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_08);
        CharacterSlot08 = saveFileWriter.LoadSaveFile();

        // Character Slot 09
        saveFileWriter.SaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_09);
        CharacterSlot09 = saveFileWriter.LoadSaveFile();

        // Character Slot 10
        saveFileWriter.SaveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_10);
        CharacterSlot10 = saveFileWriter.LoadSaveFile();
    }

    public IEnumerator LoadWorldScene() {
        // For only 1 world scene
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);
        
        // TODO: Get Back Later
        //AsyncOperation loadOperation = SceneManager.LoadSceneAsync(CurrentCharacterData.SceneIndex);

        Player.LoadDataFromCurrentCharacterData(ref CurrentCharacterData);

        yield return null;
    }

    public int GetWorldSceneIndex() {
        return worldSceneIndex;
    }
}
