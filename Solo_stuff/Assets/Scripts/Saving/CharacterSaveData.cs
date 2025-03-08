using UnityEngine;

[System.Serializable]
public class CharacterSaveData
{
    [Header("Scene Index")]
    public int SceneIndex = 1;

    [Header("Character Name")]
    public string CharacterName = "Character";

    [Header("Time Played")]
    public float secondsPlayed;

    [Header("Character Position")]
    public float xPosition;
    public float yPosition;
    public float zPosition;
}
