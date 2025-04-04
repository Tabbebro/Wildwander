using UnityEngine;
using System.Linq;

public class WorldActionManager : MonoBehaviour
{
    public static WorldActionManager Instance;

    [Header("Weapon Item Actions")]
    public WeaponItemAction[] WeaponItemActions;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        // Gives ID:s To Weapon Item Actions
        for (int i = 0; i < WeaponItemActions.Length; i++) {
            WeaponItemActions[i].actionID = i;
        }
    }

    public WeaponItemAction GetWeaponItemActionByID(int ID) { 
        return WeaponItemActions.FirstOrDefault(action => action.actionID == ID);
    }
}
