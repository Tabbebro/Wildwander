using UnityEngine;

public class PlayerInventoryManager : CharacterInventoryManager
{
    public WeaponItem CurrentRightHandWeapon;
    public WeaponItem CurrentLeftHandWeapon;

    [Header("Quick Slots")]
    public WeaponItem[] WeaponsInRightHandSlots = new WeaponItem[3];
    public int RightHandWeaponIndex = 0;
    public WeaponItem[] WeaponsInLeftHandSlots = new WeaponItem[3];
    public int LeftHandWeaponIndex = 0;
}
