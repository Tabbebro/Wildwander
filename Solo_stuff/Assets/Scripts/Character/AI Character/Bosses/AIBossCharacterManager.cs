using UnityEngine;
using Unity.Netcode;
using System.Collections;
using System.Collections.Generic;

public class AIBossCharacterManager : AICharacterManager
{
    public int BossID = 0;

    [Header("Music")]
    [SerializeField] AudioClip _bossIntroClip;
    [SerializeField] AudioClip _bossLoopClip;

    [Header("Status")]
    [SerializeField] List<FogWallInteractable> _fogWalls;
    public NetworkVariable<bool> BossFightIsActive = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> _hasBeenDefeated = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> _hasBeenAwakened = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] string _sleepAnimation;
    [SerializeField] string _awakenAnimation;

    [Header("Phase Change")]
    public int Phase02ChangeHealthPercentage = 30;
    [SerializeField] string _phaseChangeAnimation = "";
    [SerializeField] CombatStanceState _phase02CombatStanceState;


    [Header("States")]
    [SerializeField] SleepState _sleepState;


    protected override void Awake() {
        base.Awake();
    }

    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();

        BossFightIsActive.OnValueChanged += OnBossFightIsActiveChanged;
        OnBossFightIsActiveChanged(false, BossFightIsActive.Value);
        if (IsOwner) {
            _sleepState = Instantiate(_sleepState);
            _currentState = _sleepState;
        }

        if (IsServer) {
            // If No Data From Boss Add It
            if (!WorldSaveGameManager.Instance.CurrentCharacterData.BossesAwakened.ContainsKey(BossID)) {
                WorldSaveGameManager.Instance.CurrentCharacterData.BossesAwakened.Add(BossID, false);
                WorldSaveGameManager.Instance.CurrentCharacterData.BossesDefeated.Add(BossID, false);
            }
            else {
                _hasBeenDefeated.Value = WorldSaveGameManager.Instance.CurrentCharacterData.BossesDefeated[BossID];
                _hasBeenAwakened.Value = WorldSaveGameManager.Instance.CurrentCharacterData.BossesAwakened[BossID];


            }

            // Get Boss FogWalls
            StartCoroutine(GetFogWalls());

            // Check If Boss Has Been Awakened & Activate FogWalls
            if (_hasBeenAwakened.Value) {
                for (int i = 0; i < _fogWalls.Count; i++) {
                    _fogWalls[i].IsActive.Value = true;
                }
            }

            // Check If Boss Has Been Defeated & Remove Fog Walls
            if (_hasBeenDefeated.Value) {
                for (int i = 0; i < _fogWalls.Count; i++) {
                    _fogWalls[i].IsActive.Value = false;
                }
                AICharacterNetworkManager.IsActive.Value = false;
            }
        }

        if (!_hasBeenAwakened.Value) {
            Animator.Play(_sleepAnimation);
        }
    }

    public override void OnNetworkDespawn() {
        base.OnNetworkDespawn();

        BossFightIsActive.OnValueChanged -= OnBossFightIsActiveChanged;
    }

    IEnumerator GetFogWalls() {
        while (WorldObjectManager.Instance.FogWalls.Count == 0) {
            yield return new WaitForEndOfFrame();
        }

        _fogWalls = new();

        foreach (FogWallInteractable fogWall in WorldObjectManager.Instance.FogWalls) {
            if (fogWall.FogWallID == BossID) {
                _fogWalls.Add(fogWall);
            }
        }
    }

    public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false) {

        PlayerUIManager.Instance.PlayerUIPopUpManager.SendBossDefeatedPopUp("Boat Goes Binted");

        if (IsOwner) {
            if (!manuallySelectDeathAnimation) {
                CharacterAnimatorManager.PlayTargetActionAnimation("Death_01", true);
            }
            CharacterNetworkManager.CurrentHealth.Value = 0;
            IsDead.Value = true;
            BossFightIsActive.Value = false;

            foreach (FogWallInteractable fogWall in _fogWalls) {
                fogWall.IsActive.Value = false;
            }

            _hasBeenDefeated.Value = true;
            if (!WorldSaveGameManager.Instance.CurrentCharacterData.BossesAwakened.ContainsKey(BossID)) {
                WorldSaveGameManager.Instance.CurrentCharacterData.BossesAwakened.Add(BossID, true);
                WorldSaveGameManager.Instance.CurrentCharacterData.BossesDefeated.Add(BossID, true);
            }
            else {
                WorldSaveGameManager.Instance.CurrentCharacterData.BossesAwakened.Remove(BossID);
                WorldSaveGameManager.Instance.CurrentCharacterData.BossesDefeated.Remove(BossID);

                WorldSaveGameManager.Instance.CurrentCharacterData.BossesAwakened.Add(BossID, true);
                WorldSaveGameManager.Instance.CurrentCharacterData.BossesDefeated.Add(BossID, true);
            }

            WorldSaveGameManager.Instance.SaveGame();
        }


        // Play Death SFX

        yield return new WaitForSeconds(5);

        // TODO: Give Some Currency On Enemy Death

        // TODO: Disable Character
    }

    public void WakeBoss() {
        if (IsOwner) {
            if (!_hasBeenAwakened.Value) {
                CharacterAnimatorManager.PlayTargetActionAnimation(_awakenAnimation, true);
            }
            BossFightIsActive.Value = true;
            _hasBeenAwakened.Value = true;
            _currentState = Idle;

            if (!WorldSaveGameManager.Instance.CurrentCharacterData.BossesAwakened.ContainsKey(BossID)) {
                WorldSaveGameManager.Instance.CurrentCharacterData.BossesAwakened.Add(BossID, true);
            }
            else {
                WorldSaveGameManager.Instance.CurrentCharacterData.BossesAwakened.Remove(BossID);
                WorldSaveGameManager.Instance.CurrentCharacterData.BossesAwakened.Add(BossID, true);
            }

            for (int i = 0; i < _fogWalls.Count; i++) {
                _fogWalls[i].IsActive.Value = true;
            }

        }
    }

    void OnBossFightIsActiveChanged(bool oldStatus, bool newStatus) {
        if (BossFightIsActive.Value) {

            WorldSFXManager.Instance.PlayBossTrack(_bossIntroClip, _bossLoopClip);

            GameObject bossHealthBar = Instantiate(PlayerUIManager.Instance.PlayerUIHudManager.BossHealthBarObject, PlayerUIManager.Instance.PlayerUIHudManager.BossHealthBarParent);

            UI_BossHpBar bossBar = bossHealthBar.GetComponentInChildren<UI_BossHpBar>();
            bossBar.EnableBossHPBar(this);            
        }
        else {
            WorldSFXManager.Instance.StopBossMusic();
        }
    }

    public void Phase02Change() {
        CharacterAnimatorManager.PlayTargetActionAnimation(_phaseChangeAnimation, true);
        CombatStance = Instantiate(_phase02CombatStanceState);
        _currentState = CombatStance;
    }
}
