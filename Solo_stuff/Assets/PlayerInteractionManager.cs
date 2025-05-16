using UnityEngine;
using System.Collections.Generic;

public class PlayerInteractionManager : MonoBehaviour
{
    PlayerManager _player;

    [SerializeField] List<Interactable> _currentInteractableActions;

    private void Awake() {
        _player = GetComponent<PlayerManager>();
    }

    private void Start() {
        _currentInteractableActions = new();
    }

    private void FixedUpdate() {
        if (!_player.IsOwner) { return; }

        if (!PlayerUIManager.Instance.MenuWindowIsOpen && !PlayerUIManager.Instance.PopUpWindowIsOpen) {
            CheckForInteractable();
        }
    }

    void CheckForInteractable() {
        if (_currentInteractableActions.Count == 0) { return; }
        
        if (_currentInteractableActions[0] == null) {
            _currentInteractableActions.RemoveAt(0);
            return;
        }

        // Send Pop Up To Player If Haven't Already
        if (_currentInteractableActions[0] != null) {
            PlayerUIManager.Instance.PlayerUIPopUpManager.SendMessagePopUp(_currentInteractableActions[0].InteractableText);
        }

    }

    void RefreshInteractionList() {
        for (int i = _currentInteractableActions.Count - 1; i > -1; i--) {
            if (_currentInteractableActions[i] == null) {
                _currentInteractableActions.RemoveAt(i);
            }
        }
    }

    public void Interact() {
        print("Huh");
        if (_currentInteractableActions.Count <= 0) { return; }
        if (_currentInteractableActions[0] != null) {
            _currentInteractableActions[0].Interact(_player);
            RefreshInteractionList();
        }
    }

    public void AddInteractionToList(Interactable interactable) {
        RefreshInteractionList();

        if (!_currentInteractableActions.Contains(interactable)) {
            _currentInteractableActions.Add(interactable);
        }
    }

    public void RemoveInteractionFromList(Interactable interactable) {
        if (_currentInteractableActions.Contains(interactable)) {
            _currentInteractableActions.Remove(interactable);
        }

        RefreshInteractionList();
    }
}
