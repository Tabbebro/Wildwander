using UnityEngine;

public class WorldSoundFXManager : MonoBehaviour
{
    public static WorldSoundFXManager Instance;

    public AudioSource Source;

    [Header("Damage Sounds")]
    public AudioClip[] PhysicalDamageSFX;

    [Header("Action SFX")]
    public AudioClip RollSFX;

    [Header("UI")]
    public AudioClip SelectButton;
    public AudioClip ClickButton;

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

    public void PlayAudio(AudioClip clip) {
        Source.PlayOneShot(clip);
    }

    public AudioClip ChooseRandomSFXFromArray(AudioClip[] array) {
        int index = Random.Range(0, array.Length);

        return array[index];
    }
}
