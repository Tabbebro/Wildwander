using UnityEngine;

public class PlayerEquipmentManager : CharacterEquipmentManager
{
    PlayerManager _player;

    public WeaponModelInstantiationSlot RightHandSlot;
    public WeaponModelInstantiationSlot LeftHandSlot;

    public GameObject RightHandWeaponModel;
    public GameObject LeftHandWeaponModel;

    protected override void Awake() {
        base.Awake();

        _player = GetComponent<PlayerManager>();

        InitializeWeaponSlots();
    }


    protected override void Start() {
        base.Start();
        LoadWeaponsOnBothHands();
    }

    void InitializeWeaponSlots() {
        WeaponModelInstantiationSlot[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();

        foreach (WeaponModelInstantiationSlot weaponSlot in weaponSlots) {

            switch (weaponSlot.WeaponSlot) {
                case WeaponModelSlot.RightHand:
                    RightHandSlot = weaponSlot;
                    break;
                case WeaponModelSlot.LeftHand:
                    LeftHandSlot = weaponSlot;
                    break;
                case WeaponModelSlot.Back:
                    break;
                default:
                    break;
            }
        }
    }

    public void LoadWeaponsOnBothHands() {
        LoadLeftWeapon();
        LoadRightWeapon();
    }

    public void LoadRightWeapon() {
        if (_player.PlayerInventoryManager.CurrentRightHandWeapon != null) {
            RightHandWeaponModel = Instantiate(_player.PlayerInventoryManager.CurrentRightHandWeapon.weaponModel);
            RightHandSlot.LoadWeaponModel(RightHandWeaponModel);
        }
    }

    public void LoadLeftWeapon() {
        if (_player.PlayerInventoryManager.CurrentLeftHandWeapon != null) {
            LeftHandWeaponModel = Instantiate(_player.PlayerInventoryManager.CurrentLeftHandWeapon.weaponModel);
            LeftHandSlot.LoadWeaponModel(LeftHandWeaponModel);
        }
    }
}
