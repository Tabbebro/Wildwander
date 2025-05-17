using UnityEngine;
using System.Collections;
using Unity.Netcode;

public class FogWallInteractable : Interactable
{
    [Header("Fog")]
    [SerializeField] GameObject[] _fogGameObjects;

    [Header("Collision")]
    [SerializeField] Collider _fogWallCollider;

    [Header("ID")]
    public int FogWallID;

    [Header("Sound")]
    AudioSource _fogWallAudioSource;
    [SerializeField] AudioClip _fogWallSFX;

    [Header("Active")]
    public NetworkVariable<bool> IsActive = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    protected override void Awake() {
        base.Awake();

        _fogWallAudioSource = GetComponent<AudioSource>();
    }

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

    public override void Interact(PlayerManager player) {
        base.Interact(player);

        Quaternion targetRotation = Quaternion.LookRotation(-Vector3.forward);
        // TODO: Make this smooth using coroutine. This is fine for now
        player.transform.rotation = targetRotation;

        AllowPlayerThroughFogWallCollidersServerRpc(player.NetworkObjectId);
        player.PlayerAnimatorManager.PlayTargetActionAnimation("Pass_Trough_Fog_01", true);
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


    [ServerRpc(RequireOwnership = false)]
    void AllowPlayerThroughFogWallCollidersServerRpc(ulong playerObjectID) {
        if (IsServer) {
            AllowPlayerThroughFogWallCollidersClientRpc(playerObjectID);
        }
    }

    [ClientRpc]
    void AllowPlayerThroughFogWallCollidersClientRpc(ulong playerObjectID) {
        PlayerManager player = NetworkManager.Singleton.SpawnManager.SpawnedObjects[playerObjectID].GetComponent<PlayerManager>();

        _fogWallAudioSource.PlayOneShot(_fogWallSFX);

        if (player == null) { return; }

        StartCoroutine(DisableCollisionForTime(player));
    }

    private IEnumerator DisableCollisionForTime(PlayerManager player) {

        Physics.IgnoreCollision(player.CharacterController, _fogWallCollider, true);
        yield return new WaitForSeconds(3);
        Physics.IgnoreCollision(player.CharacterController, _fogWallCollider, false);
    }
}
