using System.Collections;
using UnityEngine;

public class WorldSFXManager : MonoBehaviour
{
    public static WorldSFXManager Instance;

    public AudioSource Source;

    [Header("Damage Sounds")]
    public AudioClip[] PhysicalDamageSFX;

    [Header("Action SFX")]
    public AudioClip RollSFX;

    [Header("UI")]
    public AudioClip SelectButton;
    public AudioClip ClickButton;

    #region Footsteps
    [Header("Footsteps Dirt")]
    public AudioClip[] FootstepsDirtWalk;
    public AudioClip[] FootstepsDirtRun;
    public AudioClip[] FootstepsDirtSprint;

    [Header("Footsteps Concrete")]
    public AudioClip[] FootstepsConcreteWalk;
    public AudioClip[] FootstepsConcreteRun;
    public AudioClip[] FootstepsConcreteSprint;

    [Header("Footsteps Stone")]
    public AudioClip[] FootstepsStoneWalk;
    public AudioClip[] FootstepsStoneRun;
    public AudioClip[] FootstepsStoneSprint;

    [Header("Footsteps Gravel")]
    public AudioClip[] FootstepsGravelWalk;
    public AudioClip[] FootstepsGravelRun;
    public AudioClip[] FootstepsGravelSprint;
    #endregion

    [Header("Boss Track")]
    [SerializeField] AudioSource _bossIntroPlayer;
    [SerializeField] AudioSource _bossLoopPlayer;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        DontDestroyOnLoad(gameObject);
    }

    public void PlayBossTrack(AudioClip introTrack, AudioClip loopTrack) {

        _bossIntroPlayer.volume = 1f;
        _bossIntroPlayer.loop = false;
        _bossIntroPlayer.clip = introTrack;
        _bossIntroPlayer.Play();

        _bossLoopPlayer.volume = 1f;
        _bossLoopPlayer.loop = true;
        _bossLoopPlayer.clip = loopTrack;
        _bossLoopPlayer.PlayDelayed(_bossIntroPlayer.clip.length);
    }

    public void PlayUIAudio(AudioClip clip) {
        Source.PlayOneShot(clip);
    }

    public AudioClip ChooseRandomSFXFromArray(AudioClip[] array) {
        int index = Random.Range(0, array.Length);

        return array[index];
    }

    public void StopBossMusic() {
        StartCoroutine(FadeOutBossMusic());
    }

    IEnumerator FadeOutBossMusic() {

        while (_bossLoopPlayer.volume > 0) {
            _bossLoopPlayer.volume -= Time.deltaTime;
            yield return null;
        }

        while (_bossIntroPlayer.volume > 0) {
            _bossIntroPlayer.volume -= Time.deltaTime;
            yield return null;
        }
    }
}
