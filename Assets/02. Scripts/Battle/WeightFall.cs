using System.Collections;
using UnityEngine;
using Cinemachine;
using Puzzle;
public class WeightFall : MonoBehaviour, IInstance, IPuzzleInstance
{
    public DataReader DataReader { get; private set; } = new FlowerReader();
    private int _flowerCount;
    private ParticleSystem particle;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 rotation;
    [SerializeField] private Transform _cube;
    [SerializeField] private GameObject _dust;
    [SerializeField] private NoiseSettings noiseSettings;
    [SerializeField] private CinemachineVirtualCamera cinemachine;
    [SerializeField] private Transform _bg;
    public void SetMediator(IMediatorInstance mediator)
    {
    }
    public void Init(CubePuzzleDataReader puzzleData)
    {
        _cube = puzzleData.BaseTransform;
        //camera Setting
        GameObject camera = Instantiate(cinemachine.gameObject);
        camera.SetActive(false);
        CinemachineVirtualCamera virtualCamera = camera.GetComponent<CinemachineVirtualCamera>();
        virtualCamera.LookAt = _cube;
        camera.transform.position += offset;
        camera.transform.rotation = Quaternion.Euler(rotation);

        //effect Setting
        float dust_Ypos = (_cube.localScale.y / 2) + 1;
        Vector3 dustPos = new Vector3(0, -dust_Ypos, 0);

        _dust = Instantiate(_dust);

        _dust.transform.position = dustPos;
        _dust.transform.rotation = Quaternion.Euler(new Vector3(90, 0, -50));
        _dust.transform.localScale = new Vector3(0.3f, 0.8f, 0.5f);

        particle = _dust.GetComponent<ParticleSystem>();

    }
    public void InstreamData(byte[] data)
    {
        if (FlowerReader.FLOWER_CREATE.Equals(data))
        {
            _flowerCount++;
            switch (_flowerCount)
            {
                case 3: FallLevelFunc(); break;
                case 6: FallLevelFunc(); break;
                case 9: FallLevelFunc(); break;
                case 12: FallLevelFunc(); break;
                case 15: FallLevelFunc(); break;
                case 18: FallLevelFunc(); break;
            }
        }
    }

    private void FallLevelFunc()
    {
        Vector3 pre_cube = _cube.position + Vector3.down;
        Vector3 pre_dust = _dust.transform.position + Vector3.down;
        cinemachine.gameObject.SetActive(true);
        particle.gameObject.SetActive(true);
        particle.Play();
        StartCoroutine(FallLevel(pre_cube, pre_dust));
    }
    private IEnumerator FallLevel(Vector3 pre_cube, Vector3 pre_dust)
    {
        while (true)
        {
            if (Mathf.Abs(_cube.position.y - pre_cube.y) < 0.1f)
            {
                _cube.position = pre_cube;
                _dust.transform.position = pre_dust;
                cinemachine.gameObject.SetActive(false);
                particle.gameObject.SetActive(false);
                break;
            }
            _cube.position = Vector3.MoveTowards(_cube.position, pre_cube, speed);
            _dust.transform.position = Vector3.MoveTowards(_dust.transform.position, pre_dust, speed);
            yield return null;
        }
        yield break;
    }

}
