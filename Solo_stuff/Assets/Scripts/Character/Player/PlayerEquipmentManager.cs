using UnityEngine;

public class PlayerEquipmentManager : CharacterEquipmentManager
{
    PlayerManager _player;

    public WeaponModelInstantiationSlot RightHandSlot;
    public WeaponModelInstantiationSlot LeftHandSlot;

    [SerializeField] WeaponManager _rightWeaponManager;
    [SerializeField] WeaponManager _leftWeaponManager;

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

    // Right Weapon

    public void LoadRightWeapon() {
        if (_player.PlayerInventoryManager.CurrentRightHandWeapon != null) {

            // Unload Old Weapon Model
            RightHandSlot.UnloadWeaponModel();
            // Load Model For Weapon
            RightHandWeaponModel = Instantiate(_player.PlayerInventoryManager.CurrentRightHandWeapon.weaponModel);
            RightHandSlot.LoadWeaponModel(RightHandWeaponModel);
            _rightWeaponManager = RightHandWeaponModel.GetComponent<WeaponManager>();
            _rightWeaponManager.SetWeaponDamage(_player, _player.PlayerInventoryManager.CurrentRightHandWeapon);

            // Assign Damage To Collider
        }
    }

    public void SwitchRightWeapon() {
        if (!_player.IsOwner) { return; }

        _player.PlayerAnimatorManager.PlayTargetActionAnimation("Swap_Right_Weapon_01", false, false, true, true);

        WeaponItem selectedWeapon = null;

        // TODO: Disable Two Handing Here When I Make It

        // Increment Weapon Index
        _player.PlayerInventoryManager.RightHandWeaponIndex++;

        // Clamp Index To Be Between 0 & 2
        if (_player.PlayerInventoryManager.RightHandWeaponIndex < 0 || _player.PlayerInventoryManager.RightHandWeaponIndex > 2) {
            _player.PlayerInventoryManager.RightHandWeaponIndex = 0;

            // Check If Holding More Than One Weapon
            float weaponCount = 0;
            WeaponItem firstWeapon = null;
            int firstWeaponPosition = 0;

            for (int i = 0; i < _player.PlayerInventoryManager.WeaponsInRightHandSlots.Length; i++) {
                if (_player.PlayerInventoryManager.WeaponsInRightHandSlots[i].ItemID != WorldItemDatabase.Instance.UnarmedWeapon.ItemID) {
                    weaponCount += 1;

                    if (firstWeapon == null) {
                        firstWeapon = _player.PlayerInventoryManager.WeaponsInRightHandSlots[i];
                        firstWeaponPosition = i;
                    }
                }
            }
            // If Player Is Holding 0 or 1 Item Allow Switching To Unarmed
            if (weaponCount <= 1) {
                _player.PlayerInventoryManager.RightHandWeaponIndex = -1;
                selectedWeapon = WorldItemDatabase.Instance.UnarmedWeapon;
                _player.PlayerNetworkManager.CurrentRightHandWeaponID.Value = selectedWeapon.ItemID;
            }
            // Else Go Back To First Weapon
            else {
                _player.PlayerInventoryManager.RightHandWeaponIndex = firstWeaponPosition;
                _player.PlayerNetworkManager.CurrentRightHandWeaponID.Value = firstWeapon.ItemID;
            }

            return;
        }

        foreach (WeaponItem weapon in _player.PlayerInventoryManager.WeaponsInRightHandSlots) {
            // Check If Next Weapon Is Unarmed
            if (_player.PlayerInventoryManager.WeaponsInRightHandSlots[_player.PlayerInventoryManager.RightHandWeaponIndex].ItemID != WorldItemDatabase.Instance.UnarmedWeapon.ItemID) {
                selectedWeapon = _player.PlayerInventoryManager.WeaponsInRightHandSlots[_player.PlayerInventoryManager.RightHandWeaponIndex];
                // Assign Network Weapon ID So It Changes To All Clients
                _player.PlayerNetworkManager.CurrentRightHandWeaponID.Value = _player.PlayerInventoryManager.WeaponsInRightHandSlots[_player.PlayerInventoryManager.RightHandWeaponIndex].ItemID;
                return;
            }
        }

        if (selectedWeapon == null && _player.PlayerInventoryManager.RightHandWeaponIndex <= 2) {
            SwitchRightWeapon();
        }
    }

    // Left Weapon

    public void LoadLeftWeapon() {
        if (_player.PlayerInventoryManager.CurrentLeftHandWeapon != null) {

            // Unload Old Weapon Model
            LeftHandSlot.UnloadWeaponModel();


            // Load Model For Weapon
            LeftHandWeaponModel = Instantiate(_player.PlayerInventoryManager.CurrentLeftHandWeapon.weaponModel);
            LeftHandSlot.LoadWeaponModel(LeftHandWeaponModel);
            _leftWeaponManager = LeftHandWeaponModel.GetComponent<WeaponManager>();
            _leftWeaponManager.SetWeaponDamage(_player, _player.PlayerInventoryManager.CurrentLeftHandWeapon);
            
            // Assign Damage To Collider

        }
    }
}
