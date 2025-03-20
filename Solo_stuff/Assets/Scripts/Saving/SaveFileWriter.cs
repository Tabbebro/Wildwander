using UnityEngine;
using System;
using System.IO;
using System.Linq.Expressions;

public class SaveFileWriter
{
    public string SaveDataPath = "";
    public string SaveFileName = "";

    public bool CheckToSeeIfFileExists() {
        if (File.Exists(Path.Combine(SaveDataPath, SaveFileName))) {
            return true;
        }
        else {
            return false;
        }
    }

    public void DeleteSaveFile() {
        File.Delete(Path.Combine(SaveDataPath, SaveFileName));
    }

    public void CreateNewCharacterSaveFile(CharacterSaveData characterData) {

        string savePath = Path.Combine(SaveDataPath, SaveFileName);

        try {
            //  Create Directory
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            Debug.Log("Creating Save File, At Path: " + savePath);

            // Make Json
            string dataToStore = JsonUtility.ToJson(characterData, true);

            // Save Json
            using (FileStream stream = new FileStream(savePath, FileMode.Create)) {
                using (StreamWriter filreWriter = new StreamWriter(stream)) {
                    filreWriter.Write(dataToStore);
                }
            }
        }
        catch (Exception ex){
            // Show error if happens
            Debug.LogError("Error While Trying To Save Character Data, Game Not Saved: " + savePath + "\n" + ex);
        }
    }

    public CharacterSaveData LoadSaveFile() {

        CharacterSaveData characterData = null;

        // Get Path Of File
        string loadPath = Path.Combine(SaveDataPath, SaveFileName);

        // Check If File Exists
        if (File.Exists(loadPath)) {
            try {
                string dataToLoad = "";

                // Load Data
                using (FileStream stream = new FileStream(loadPath, FileMode.Open)) {
                    using (StreamReader reader = new StreamReader(stream)) {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // Turn Data Back To CharacterData From Json
                characterData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
            }
            catch (Exception ex) {
                Debug.LogError("Error While Trying To Load Character Data, Game Not Loaded: " + loadPath + "\n" + ex);
            }

        }
        
        return characterData;
    }
}
