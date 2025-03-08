using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSaveGameManager : MonoBehaviour
{
    public static WorldSaveGameManager Instance;

    [SerializeField] PlayerManager _player;

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
    //public CharacterSaveData CharacterSlot02;
    //public CharacterSaveData CharacterSlot03;
    //public CharacterSaveData CharacterSlot04;
    //public CharacterSaveData CharacterSlot05;
    //public CharacterSaveData CharacterSlot06;
    //public CharacterSaveData CharacterSlot07;
    //public CharacterSaveData CharacterSlot08;
    //public CharacterSaveData CharacterSlot09;
    //public CharacterSaveData CharacterSlot10;

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

    void DecideCharacterFileNameBasedOnCharacterSlotBeingUsed() {
        switch (CurrentCharacterSlotUsed) {
            case CharacterSlot.CharacterSlot_01:
                _fileName = "Slot_01";
                break;
            case CharacterSlot.CharacterSlot_02:
                _fileName = "Slot_02";
                break;
            case CharacterSlot.CharacterSlot_03:
                _fileName = "Slot_03";
                break;
            case CharacterSlot.CharacterSlot_04:
                _fileName = "Slot_04";
                break;
            case CharacterSlot.CharacterSlot_05:
                _fileName = "Slot_05";
                break;
            case CharacterSlot.CharacterSlot_06:
                _fileName = "Slot_06";
                break;
            case CharacterSlot.CharacterSlot_07:
                _fileName = "Slot_07";
                break;
            case CharacterSlot.CharacterSlot_08:
                _fileName = "Slot_08";
                break;
            case CharacterSlot.CharacterSlot_09:
                _fileName = "Slot_09";
                break;
            case CharacterSlot.CharacterSlot_10:
                _fileName = "Slot_10";
                break;
            default:
                break;
        }
    }

    public void NewGame() {
        DecideCharacterFileNameBasedOnCharacterSlotBeingUsed();

        CurrentCharacterData = new CharacterSaveData();
    }

    public void LoadGame() {
        DecideCharacterFileNameBasedOnCharacterSlotBeingUsed();

        saveFileWriter = new();
        saveFileWriter.SaveDataPath = Application.persistentDataPath;
        saveFileWriter.SaveFileName = _fileName;

        // Loads Save File
        CurrentCharacterData = saveFileWriter.LoadSaveFile();

        // Loads World Scene
        StartCoroutine(LoadWorldScene());
    }

    public void SaveGame() {
        DecideCharacterFileNameBasedOnCharacterSlotBeingUsed();

        saveFileWriter = new();
        saveFileWriter.SaveDataPath = Application.persistentDataPath;
        saveFileWriter.SaveFileName = _fileName;

        // Get Values From Player
        _player.SaveDataToCurrentCharacterData(ref CurrentCharacterData);

        // Write Info To Save File
        saveFileWriter.CreateNewCharacterSaveFile(CurrentCharacterData);
    }

    public IEnumerator LoadWorldScene() {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);
        
        yield return null;
    }

    public int GetWorldSceneIndex() {
        return worldSceneIndex;
    }
}
