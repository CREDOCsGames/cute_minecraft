using System.Collections;
using UnityEngine;
using Cinemachine;
using Puzzle;
public class WeightFall : MonoBehaviour, IInstance, IPuzzleInstance
{
    public DataReader DataReader { get; private set; } = new FlowerReader();
    private int _flowerCount;
    private bool _falling = false;
    [SerializeField] private float speed;
    [SerializeField] private CinemachineVirtualCamera cinemachine;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private Transform _bg;
    public void SetMediator(IMediatorInstance mediator)
    {
    }
    public void Init(CubePuzzleDataReader puzzleData)
    {
        cinemachine.LookAt = puzzleData.BaseTransform;
    }
    public void InstreamData(byte[] data)
    {
        if (MonsterReader.BOSS_SPIT_OUT_SUCCESS.Equals(data)) 
        {
            _flowerCount++;
            if(_flowerCount < 19 && _flowerCount % 3 ==0)
            {
                FallCube();
            }
        }
    }
    private void FallCube()
    {
        Vector3 pre_bg = _bg.position + Vector3.up;
        cinemachine.gameObject.SetActive(true);
        particle.gameObject.SetActive(true);
        particle.Play();
        StartCoroutine(FallLevel(pre_bg));
    }
    private IEnumerator FallLevel(Vector3 pre_bg)
    {
        if (_falling) { yield break; }
        _falling = true;
        while (true)
        {
            if (Mathf.Abs(_bg.position.y - pre_bg.y) < 0.1f)
            {
                cinemachine.gameObject.SetActive(false);
                particle.gameObject.SetActive(false);
                _falling = false;
                break;
            }
            _bg.position = Vector3.MoveTowards(_bg.position, pre_bg,speed);
            yield return null;
        }
        yield break;
    }

}
