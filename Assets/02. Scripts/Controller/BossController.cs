using System;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [NonSerialized] public Vector3 SlimeSpawnPoint;
    private BossSlimeSpawner _slimeSpawner;
    public event Action OnFailed;

    void Start()
    {
        _slimeSpawner = GetComponent<BossSlimeSpawner>();
        _slimeSpawner.OnFailed += ()=>OnFailed?.Invoke();
    }
    public void OnSlimeSpawnAnimationEvent()
    {
        _slimeSpawner.SpawnAt(SlimeSpawnPoint);
    }
    public void Success()
    {
        _slimeSpawner.OnSuccess();
    }
    public void ClearMonster()
    {
        _slimeSpawner.ClearMonster();
    }
}