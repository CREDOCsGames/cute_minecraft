using System.Collections;
using UnityEngine;
using Cinemachine;
using Puzzle;
using Sound;

public class WeightFall : MonoBehaviour, IInstance, IPuzzleInstance
{
    public DataReader DataReader { get; private set; } = new MonsterReader();
    private int _flowerCount;
    private Vector3 _preBgPos;
    private int _fallCount => ((int)_fallChargeLimit / (int)_fallCharge);
    [SerializeField] private float _duration;
    [SerializeField] private CinemachineVirtualCamera _shakeCamera;
    [SerializeField] private ParticleSystem _dust;
    [SerializeField] private Transform _bg;
    [SerializeField, Range(1, 255)] private byte _fallCharge = 3;
    [SerializeField, Range(1, 255)] private byte _fallChargeLimit = 19;
    [SerializeField, Range(1, 1000)] private float _fallHeight = 5;
    public void SetMediator(IMediatorInstance mediator)
    {
    }
    public void InitInstance(CubePuzzleReader puzzleData)
    {
        _preBgPos = _bg.transform.position;
        StartCoroutine(UpLevel());
    }
    public void InstreamData(byte[] data)
    {
        if (MonsterReader.BOSS_SPIT_OUT_SUCCESS.Equals(data))
        {
            _flowerCount++;
            if (_flowerCount <= _fallChargeLimit &&
                _flowerCount % _fallCharge == 0)
            {
                FallCube();
            }
        }
    }
    private void FallCube()
    {
        Vector3 targetPos = _bg.position + Vector3.up * _fallHeight / _fallCount;
        StartCoroutine(FallLevel(targetPos));
    }
    private IEnumerator FallLevel(Vector3 targetPos)
    {
        float t = 0;
        var prePos = _bg.transform.position;
        _shakeCamera.gameObject.SetActive(true);
        _dust.gameObject.SetActive(true);
        _dust.Play();
        SoundManagerComponent.Instance.PlaySound("Move_Stone");
        while (t < 1)
        {
            t = Mathf.Clamp01(t + Time.deltaTime / _duration);
            _bg.position = Vector3.Lerp(prePos, targetPos, t);
            yield return true;
        }
        _shakeCamera.gameObject.SetActive(false);
        _dust.gameObject.SetActive(false);
    }
    private IEnumerator UpLevel()
    {
        float t = 0;
        while (t < 1)
        {
            t = Mathf.Clamp01(t + Time.deltaTime / _duration);
            _bg.position = Vector3.Lerp(_preBgPos, _preBgPos + Vector3.down * _fallHeight, t);
            yield return true;
        }
    }
}
