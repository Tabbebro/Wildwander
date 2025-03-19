using UnityEngine;

public class WeaponModelInstantiationSlot : MonoBehaviour
{
    public WeaponModelSlot WeaponSlot;
    public GameObject CurrentWeaponModel;

    public void UnloadWeaponModel() {
        if (CurrentWeaponModel != null) {
            Destroy(CurrentWeaponModel);
        }
    }

    public void LoadWeaponModel(GameObject weaponModel) {
        CurrentWeaponModel = weaponModel;
        weaponModel.transform.parent = transform;

        weaponModel.transform.localPosition = Vector3.zero;
        weaponModel.transform.localRotation = Quaternion.identity;
        weaponModel.transform.localScale = Vector3.one;
    }
}
