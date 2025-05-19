using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class SpendTimeWindow : EditorWindow
{
    int timesProjectOpened = 0, timesProjectCompiled = 0, timesPlayModePressed = 0;
    float totalTimeSpent = 0f, currentSessionLength = 0f;
    double totalDomainReloadTime = 0f, totalPlayModeTime = 0f;

    [MenuItem("Window/Spend Time")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(SpendTimeWindow));
    } 
    private void OnGUI()
    {
        GetTimes();
        SetLabels();
    }
    void GetTimes()
    {
        timesProjectOpened = LoadManager.Instance.Opened();
        timesProjectCompiled = LoadManager.Instance.Compiled();
        timesPlayModePressed = LoadManager.Instance.PlayPressed();
        totalTimeSpent = LoadManager.Instance.TotalTime();
        totalDomainReloadTime = LoadManager.Instance.TotalDomainReloadTime();
        totalPlayModeTime = LoadManager.Instance.TotalPlayModeTime();
        currentSessionLength = (float)(EditorApplication.timeSinceStartup);
    }
    void SetLabels()
    {
        GUILayout.Label($"Times project opened: {timesProjectOpened}", EditorStyles.boldLabel);
        GUILayout.Label($"Times Compiled: {timesProjectCompiled}", EditorStyles.boldLabel);
        GUILayout.Label($"Times play mode entered: {timesPlayModePressed}", EditorStyles.boldLabel);
        GUILayout.Label($"Total project time: {TimeSpan.FromSeconds(totalTimeSpent).Days} D " +
            $"{TimeSpan.FromSeconds(totalTimeSpent).Hours} H " +
            $"{TimeSpan.FromSeconds(totalTimeSpent).Minutes} mm " +
            $"{TimeSpan.FromSeconds(totalTimeSpent).Seconds} ss ", EditorStyles.boldLabel);

        GUILayout.Label($"Current session length: {TimeSpan.FromSeconds(currentSessionLength).Days} D " +
            $"{TimeSpan.FromSeconds(currentSessionLength).Hours} H " +
            $"{TimeSpan.FromSeconds(currentSessionLength).Minutes} mm " +
            $"{TimeSpan.FromSeconds(currentSessionLength).Seconds} ss ", EditorStyles.boldLabel);

        GUILayout.Label($"Total time wasted on domain reload: {TimeSpan.FromSeconds(totalDomainReloadTime).Days} D " +
            $"{TimeSpan.FromSeconds(totalDomainReloadTime).Hours} H " +
            $"{TimeSpan.FromSeconds(totalDomainReloadTime).Minutes} mm " +
            $"{TimeSpan.FromSeconds(totalDomainReloadTime).Seconds} ss ", EditorStyles.boldLabel);

        GUILayout.Label($"Total time in play mode: {TimeSpan.FromSeconds(totalPlayModeTime).Days} D " +
            $"{TimeSpan.FromSeconds(totalPlayModeTime).Hours} H " +
            $"{TimeSpan.FromSeconds(totalPlayModeTime).Minutes} mm " +
            $"{TimeSpan.FromSeconds(totalPlayModeTime).Seconds} ss ", EditorStyles.boldLabel);
    }
}
[InitializeOnLoad]
static class OnStartUp
{
    private const string ReloadStartTimeKey = "\"DomainReloadProfiler.StartTime";
    private const string PlayModeTimeKey = "\"PlayMode.StartTime";
    static OnStartUp()
    {
        AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyRelaod;
        AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyRelaod;

        EditorApplication.playModeStateChanged += ModeStateChanged;
        EditorApplication.quitting += OnQuit;

        LoadManager.Instance.IncrementCompiled();

        if (!SessionState.GetBool("FirstInitDone", false)) // Get the first startup
        {
            LoadManager.Instance.IncrementOpened();
            SessionState.SetBool("FirstInitDone", true);
        }

        LoadManager.Instance.Save();
    }
    #region Event subscribers
    private static void OnBeforeAssemblyRelaod()
    {
        long startTimestamp = Stopwatch.GetTimestamp();
        EditorPrefs.SetString(ReloadStartTimeKey, startTimestamp.ToString());
    }
    private static void OnAfterAssemblyRelaod()
    {
        if (EditorPrefs.HasKey(ReloadStartTimeKey))
        {
            long startTimestamp = long.Parse(EditorPrefs.GetString(ReloadStartTimeKey));
            long endTimestamp = Stopwatch.GetTimestamp();
            long elapsedTicks = endTimestamp - startTimestamp;
            double elapsedSeconds = (double)elapsedTicks / Stopwatch.Frequency;

            LoadManager.Instance.AddDomainReloadTime(elapsedSeconds);

            UnityEngine.Debug.Log($"Domain reload time: {elapsedSeconds:F2} seconds");

            EditorPrefs.DeleteKey(ReloadStartTimeKey);
            LoadManager.Instance.Save();
        }
        else
        {
            UnityEngine.Debug.LogWarning("Domain reload start time was not found.");
        }
    }
    private static void OnQuit()
    {
        LoadManager.Instance.AddTime((float)(EditorApplication.timeSinceStartup));
        LoadManager.Instance.Save();
    }

    private static void ModeStateChanged(PlayModeStateChange change)
    {
        
        if (change == PlayModeStateChange.EnteredPlayMode)
        {
            long playModestartTimestamp = Stopwatch.GetTimestamp();
            EditorPrefs.SetString(PlayModeTimeKey, playModestartTimestamp.ToString());
        }
        if (change == PlayModeStateChange.ExitingPlayMode)
        {
            if (EditorPrefs.HasKey(PlayModeTimeKey))
            {
                long playModestartTimestamp = long.Parse(EditorPrefs.GetString(PlayModeTimeKey));
                long endTimestamp = Stopwatch.GetTimestamp();
                long elapsedTicks = endTimestamp - playModestartTimestamp;
                double elapsedSeconds = (double)elapsedTicks / Stopwatch.Frequency;

                LoadManager.Instance.AddPlayModeTime(elapsedSeconds);

                UnityEngine.Debug.Log($"Play mode time: {elapsedSeconds:F2} seconds");

                EditorPrefs.DeleteKey(PlayModeTimeKey);
                LoadManager.Instance.Save();
            }
            else
            {
                UnityEngine.Debug.LogWarning("Play mode start time was not found.");
            }
        }
        if (change == PlayModeStateChange.ExitingEditMode)
        {
            LoadManager.Instance.IncrementPlayPressed();
            LoadManager.Instance.IncrementCompiled();
            LoadManager.Instance.Save();
        }
    }
    #endregion
}


