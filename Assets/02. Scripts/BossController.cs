using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public Transform slimeSpawnPoint;
    private BossSlimeSpawner slimeSpawner;

    void Start()
    {
        slimeSpawner = GetComponent<BossSlimeSpawner>();
    }

    public void OnSlimeSpawnAnimationEvent()
    {
        Vector3 spawnPosition = slimeSpawnPoint.position;

        slimeSpawner.ReplaceSlime(spawnPosition);
    }
}