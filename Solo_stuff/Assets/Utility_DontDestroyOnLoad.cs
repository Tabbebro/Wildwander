using UnityEngine;

public class Utility_DontDestroyOnLoad : MonoBehaviour
{
    private void Awake() {
        DontDestroyOnLoad(this);
    }
}
