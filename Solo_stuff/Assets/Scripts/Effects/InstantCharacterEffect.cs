using UnityEngine;

public class InstantCharacterEffect : ScriptableObject
{
    [Header("Effect ID")]
    public int InstantEffectID;

    public virtual void ProcessEffect(CharacterManager character) {

    }
}
