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

    [Header("Default Footsteps")]
    public AudioClip[] FootstepsDirt;
    public AudioClip[] FootstepsConcrete;
    public AudioClip[] FootstepsStone;
    public AudioClip[] FootstepsGravel;

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
    public AudioClip ChooseRandomFootstepBasedOnGround(GameObject steppedOnObject, CharacterManager character) {
        switch (steppedOnObject.tag) {
            case "Untagged":    return ChooseRandomSFXFromArray(character.CharacterSFXManager.FootstepsDirt);
            case "Dirt":        return ChooseRandomSFXFromArray(character.CharacterSFXManager.FootstepsDirt);
            case "Concrete":    return ChooseRandomSFXFromArray(character.CharacterSFXManager.FootstepsConcrete);
            case "Stone":       return ChooseRandomSFXFromArray(character.CharacterSFXManager.FootstepsStone);
            case "Gravel":      return ChooseRandomSFXFromArray(character.CharacterSFXManager.FootstepsGravel);
            default:            return null;
        }
    }
}
