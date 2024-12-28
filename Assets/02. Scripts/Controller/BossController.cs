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

        // 슬라임 스포너의 슬라임을 생성 좌표로 대체한다.
        slimeSpawner.SpawnAt(spawnPosition);
    }
}