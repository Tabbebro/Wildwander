using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldItemDatabase : MonoBehaviour
{
    public static WorldItemDatabase Instance;

    public WeaponItem UnarmedWeapon;

    [Header("Weapons")]
    [SerializeField] List<WeaponItem> _weapons = new();

    [Header("Items")]
    private List<Item> _items = new();

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }

        foreach (WeaponItem weapon in _weapons) {
            _items.Add(weapon);
        }

        // Asigns items ID
        for (int i = 0; i < _items.Count; i++) {
            _items[i].ItemID = i;
        }
    }

    public WeaponItem GetWeaponByID(int ID) {
        return _weapons.FirstOrDefault(weapon => weapon.ItemID == ID);
    }
}
