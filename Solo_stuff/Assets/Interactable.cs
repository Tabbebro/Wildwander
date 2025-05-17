using UnityEngine;
using Unity.Netcode;

public class Interactable : NetworkBehaviour
{
    public string InteractableText;
    [SerializeField] protected Collider _interactableCollider;
    [SerializeField] protected bool _hostOnlyInteractable = true;
    
    protected virtual void Awake() {
        if (_interactableCollider == null) {
            _interactableCollider = GetComponent<Collider>();
        }
    }

    protected virtual void Start() {

    }

    public virtual void Interact(PlayerManager player) {
        if (!player.IsOwner) { return; }

        _interactableCollider.enabled = false;

        player.PlayerInteractionManager.RemoveInteractionFromList(this);

        PlayerUIManager.Instance.PlayerUIPopUpManager.CloseAllPopUpWindows();
    }

    public virtual void OnTriggerEnter(Collider other) {
        PlayerManager player = other.GetComponent<PlayerManager>();

        if (player != null) {
            if (!player.PlayerNetworkManager.IsHost && _hostOnlyInteractable) { return; }
            if (!player.IsOwner) { return; }

            player.PlayerInteractionManager.AddInteractionToList(this);
        }
    }
    public virtual void OnTriggerExit(Collider other) {
        PlayerManager player = other.GetComponent<PlayerManager>();
        if (player != null) {
            if (!player.PlayerNetworkManager.IsHost && _hostOnlyInteractable) { return; }
            if (!player.IsOwner) { return; }


            player.PlayerInteractionManager.RemoveInteractionFromList(this);

            PlayerUIManager.Instance.PlayerUIPopUpManager.CloseAllPopUpWindows();
        }
    }
}
