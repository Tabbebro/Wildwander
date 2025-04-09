using UnityEngine;

public class Utility_DestroyAfterTime : MonoBehaviour
{
    [SerializeField] float _timeUntilDestroyed = 5;
    private void Awake() {
        Destroy(gameObject, _timeUntilDestroyed);
    }
}
