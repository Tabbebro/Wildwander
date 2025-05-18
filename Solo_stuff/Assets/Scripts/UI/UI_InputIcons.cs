using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UI_InputIcons
{
    public string ActionName;
    public Sprite KeyboardIcon;
    public Sprite Ps4Icon;
    public Sprite XboxIcon;
}

[CreateAssetMenu(fileName = "Input Icon Database", menuName = "Input/Icon Database")]
public class InputIconDatabase : ScriptableObject {
    public List<UI_InputIcons> Mappings;
}
