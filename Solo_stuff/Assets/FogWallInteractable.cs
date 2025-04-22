using UnityEngine;
using Unity.Netcode;

public class FogWallInteractable : NetworkBehaviour
{
    [Header("Fog")]
    [SerializeField] GameObject[] _fogGameObjects;

    [Header("ID")]
    public int FogWallID;

    [Header("Active")]
    public NetworkVariable<bool> IsActive = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();

        OnIsActiveChanged(false, IsActive.Value);
        IsActive.OnValueChanged += OnIsActiveChanged;
        WorldObjectManager.Instance.AddFogWallToList(this);
    }
    public override void OnNetworkDespawn() {
        base.OnNetworkDespawn();

        IsActive.OnValueChanged -= OnIsActiveChanged;
        WorldObjectManager.Instance.RemoveFogWallFromList(this);
    }


    void OnIsActiveChanged(bool oldStatus, bool newStatus) {
        if (IsActive.Value) {
            foreach (GameObject fogObject in _fogGameObjects) {
                fogObject.SetActive(true);
            }
        }
        else {
            foreach (GameObject fogObject in _fogGameObjects) {
                fogObject.SetActive(false);
            }
        }
    }
}
