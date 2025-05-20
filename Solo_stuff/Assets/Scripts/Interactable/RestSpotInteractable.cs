using Unity.Netcode;
using UnityEngine;
using System.Collections;

public class RestSpotInteractable : Interactable
{
    [Header("Rest Spot")]
    [SerializeField] int _restSpotID;

    [Header("Activated")]
    public NetworkVariable<bool> IsActivated = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Interaction Text")]
    [SerializeField] string _unactivatedInteractionText = "Touch The Orb";
    [SerializeField] string _activatedInteractionText = "Ponder The Orb";

    [Header("References")]
    [SerializeField] Renderer _restSpotRenderer;
    [SerializeField] GameObject _restSpotParticles;
    [SerializeField] Light _restSpotLight;
    [SerializeField] Animation _lightAnimation;

    [Header("Activated Variables")]
    [SerializeField] Material _activatedRestSpotMaterial;
    [SerializeField] Material _unactivatedRestSpotMaterial;
    [SerializeField] AnimationClip _unactivatedLightAnimation;
    [SerializeField] AnimationClip _activatedLightAnimation;
    [SerializeField] float _restSpotLightUnactivatedRange = 1;
    [SerializeField] float _restSpotLightActivadeRange = 5;

    protected override void Start() {
        base.Start();

        if (!IsOwner) { return; }

        if (WorldSaveGameManager.Instance.CurrentCharacterData.RestSpot.ContainsKey(_restSpotID)) {
            IsActivated.Value = WorldSaveGameManager.Instance.CurrentCharacterData.RestSpot[_restSpotID];
        }
        else {
            IsActivated.Value = false;
        }


    }

    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();

        // For Syncing
        if (!IsOwner) { OnIsActivatedChange(false, IsActivated.Value); }

        IsActivated.OnValueChanged += OnIsActivatedChange;
    }

    public override void OnNetworkDespawn() {
        base.OnNetworkDespawn();

        IsActivated.OnValueChanged -= OnIsActivatedChange;
    }

    void ActivateRestSpot(PlayerManager player) {
        IsActivated.Value = true;

        // Remove Old Unactive Rest Spot Key
        if (WorldSaveGameManager.Instance.CurrentCharacterData.RestSpot.ContainsKey(_restSpotID)) {
            WorldSaveGameManager.Instance.CurrentCharacterData.RestSpot.Remove(_restSpotID);
        }
        // Add New Active Rest Spot Key
        WorldSaveGameManager.Instance.CurrentCharacterData.RestSpot.Add(_restSpotID, true);

        player.PlayerAnimatorManager.PlayTargetActionAnimation("Activate_Rest_Spot_01", true);
        // TODO: Hide Weapon Models During Animation

        PlayerUIManager.Instance.PlayerUIPopUpManager.SendRestSpotActivatedPopUp();

        StartCoroutine(WaitForAnimationAndPopUpThenRestoreCollider());
    }

    void RestAtRestSpot(PlayerManager player) {
        Debug.Log("Resting");

        // TODO: Remove Later This Is Temporary
        player.PlayerNetworkManager.CurrentHealth.Value = player.PlayerNetworkManager.MaxHealth.Value;
        player.PlayerNetworkManager.CurrentStamina.Value = player.PlayerNetworkManager.MaxStamina.Value;

        _interactableCollider.enabled = true;
        // Temp Code Ends Here

        WorldAIManager.Instance.ResetAllCharacters();

    }

    IEnumerator WaitForAnimationAndPopUpThenRestoreCollider() {
        yield return new WaitForSeconds(2);
        _interactableCollider.enabled = true;
    }

    public override void Interact(PlayerManager player) {
        base.Interact(player);

        if (IsActivated.Value) {
            RestAtRestSpot(player);
        }
        else {
            ActivateRestSpot(player);
        }
    }

    void OnIsActivatedChange(bool oldStatus, bool newStatus) {
        _restSpotParticles.SetActive(IsActivated.Value);

        if (IsActivated.Value) {
            _restSpotRenderer.material = _activatedRestSpotMaterial;
            _restSpotLight.range = _restSpotLightActivadeRange;
            InteractableText = _activatedInteractionText;
        }
        else {
            _restSpotRenderer.material = _unactivatedRestSpotMaterial;
            _restSpotLight.range = _restSpotLightUnactivatedRange;
            InteractableText = _unactivatedInteractionText;
        }
    }
}
