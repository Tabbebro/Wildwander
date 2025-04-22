using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<Tkey, TValue> : Dictionary<Tkey, TValue>, ISerializationCallbackReceiver 
{
    [SerializeField] List<Tkey> _keys = new();
    [SerializeField] List<TValue> _values = new();
    public void OnBeforeSerialize() {
        _keys.Clear(); 
        _values.Clear();

        foreach (KeyValuePair<Tkey, TValue> pair in this) {
            _keys.Add(pair.Key);
            _values.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize() {
        Clear();

        if (_keys.Count != _values.Count) {
            Debug.LogError("Key Count Doesn't Match Values Count. Something Is Wrong Pls Check");
        }

        for (int i = 0; i < _keys.Count; i++) {
            Add(_keys[i], _values[i]);
        }
    }

}
