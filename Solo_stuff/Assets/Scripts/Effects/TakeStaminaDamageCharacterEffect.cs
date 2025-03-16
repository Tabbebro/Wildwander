using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Stamina Damage")]
public class TakeStaminaDamageEffect : InstantCharacterEffect
{
    public float StaminaDamage;
    public override void ProcessEffect(CharacterManager character) {

        CalculateStaminaDamage(character);
    }

    private void CalculateStaminaDamage(CharacterManager character) {
        if (character.IsOwner) {
            character.CharacterNetworkManager.CurrentStamina.Value -= StaminaDamage;
        }
    }
}
