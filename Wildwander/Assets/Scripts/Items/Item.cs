using UnityEngine;

public class Item : ScriptableObject
{
    [Header("Item Info")]
    public string ItemName;
    public Sprite ItemIcon;
    [TextArea] public string ItemDescription;
    public int ItemID;


}
