using Codice.Utils;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadManager
{
    #region Instance
    private static LoadManager instance;
    public static LoadManager Instance => instance ??= new LoadManager();
    #endregion

    double totalDomainReloadTime = 0, totalTimeSpentInPlayMode = 0;
    int timesProjectOpened = 0, timesCompiled = 0, timesPlayModePressed = 0;
    float totalTimeSpent = 0;
    string saveFileName = Path.Combine(Application.persistentDataPath + "/spentTimeData.json");
    public LoadManager()
    {
        Load();
    }
    #region Saving and loading
    void Load()
    {
        if (File.Exists(saveFileName))
        {
            string jsonDataLoad = File.ReadAllText(saveFileName);
            SpendDataList dataLoaded = JsonUtility.FromJson<SpendDataList>(jsonDataLoad);

            if (dataLoaded.spendDatas.Count > 0)
            {
                var item = dataLoaded.spendDatas[0];
                timesProjectOpened = item.TimesOpened;
                timesCompiled = item.TimesCompiled;
                timesPlayModePressed = item.TimesPlayModePressed;
                totalTimeSpent = item.TotalTimeProjectOpen;
                totalDomainReloadTime = item.TotalDomainReloadTime;
                totalTimeSpentInPlayMode = item.TotalPlayModeTime;
            }
        }
    }
    public void Save()
    {
        SpendDataList dataToSave = new SpendDataList();
        dataToSave.spendDatas.Add(new SpendData
        {
            TimesOpened = timesProjectOpened,
            TimesCompiled = timesCompiled,
            TimesPlayModePressed = timesPlayModePressed,
            TotalTimeProjectOpen = totalTimeSpent,
            TotalDomainReloadTime = totalDomainReloadTime,
            TotalPlayModeTime = totalTimeSpentInPlayMode
        });

        string json = JsonUtility.ToJson(dataToSave, true);
        File.WriteAllText(saveFileName, json);
    }
    #endregion
    #region Setters
    // Setters / Incrementers
    public void IncrementOpened() => timesProjectOpened++;
    public void IncrementCompiled() => timesCompiled++;
    public void IncrementPlayPressed() => timesPlayModePressed++;
    public void AddTime(float seconds) => totalTimeSpent += seconds;
    public void AddDomainReloadTime(double seconds) => totalDomainReloadTime += seconds;
    public void AddPlayModeTime(double seconds) => totalTimeSpentInPlayMode += seconds;
    #endregion
    #region Getters
    // Getters
    public int Opened() => timesProjectOpened;
    public int Compiled() => timesCompiled;
    public int PlayPressed() => timesPlayModePressed;
    public float TotalTime() => totalTimeSpent;
    public double TotalDomainReloadTime() => totalDomainReloadTime;
    public double TotalPlayModeTime() => totalTimeSpentInPlayMode;
    #endregion
}
#region Save Data
[System.Serializable]
public class SpendData
{
    public int TimesOpened;
    public int TimesCompiled;
    public int TimesPlayModePressed;
    public float TotalTimeProjectOpen;
    public double TotalDomainReloadTime;
    public double TotalPlayModeTime;
}
[System.Serializable]
public class SpendDataList
{
    public List<SpendData> spendDatas = new List<SpendData>();
}
#endregion