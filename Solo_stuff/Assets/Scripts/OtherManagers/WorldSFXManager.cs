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

    public void PlayUIAudio(AudioClip clip) {
        Source.PlayOneShot(clip);
    }

    public AudioClip ChooseRandomSFXFromArray(AudioClip[] array) {
        int index = Random.Range(0, array.Length);

        return array[index];
    }
}
