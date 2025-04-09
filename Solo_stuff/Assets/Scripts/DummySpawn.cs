using UnityEngine;
using Unity.Netcode;
using System.Collections;

public class DummySpawn : MonoBehaviour
{
    NetworkObject _netObj;
    void Start()
    {
        _netObj = GetComponent<NetworkObject>();
        _netObj.Spawn();
    }
}
