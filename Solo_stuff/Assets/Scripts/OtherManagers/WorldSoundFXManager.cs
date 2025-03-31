using UnityEngine;

public class WorldSoundFXManager : MonoBehaviour
{
    public static WorldSoundFXManager Instance;

    [Header("Damage Sounds")]
    public AudioClip[] PhysicalDamageSFX;


    [Header("Action SFX")]
    public AudioClip rollSFX;

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

    public AudioClip ChooseRandomSFXFromArray(AudioClip[] array) {
        int index = Random.Range(0, array.Length);

        return array[index];
    }
}
