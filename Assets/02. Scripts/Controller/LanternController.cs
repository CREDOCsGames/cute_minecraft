using Puzzle;
using System.Collections;
using UnityEngine;
using static SuperCharacterController;

public class LanternController : MonoBehaviour, IInstance, IPuzzleInstance, IDestroyable
{
    public DataReader DataReader { get; private set; } = new SystemReader();

    public float MoveTime;
    public float LanternInterval;
    public float LanternDelayTime;
    public Vector3 LanternMoveHight;

    public Vector3 FrontSide;
    public Vector3 LeftSide;
    public Vector3 RightSide;
    public Vector3 BackSide;
    public bool IsDisapear { get; private set; } = false;

    private CubePuzzleDataReader _castingPuzzleData;
    private Animator _animator;
    private Vector3 _cubeCenter;
    private byte _cubeWidth;
    private bool _onRotated;
    private BoxCollider _collider;

    public void Init(CubePuzzleDataReader puzzleData)
    {
        this.gameObject.SetActive(true);
        _castingPuzzleData = puzzleData;
        _castingPuzzleData.OnRotatedStage += SetLenternPosition;
        _cubeWidth = puzzleData.Width;
        SetSpawnPoint(_castingPuzzleData);

        float x = LanternInterval*2 + 0.8f;
        _collider.size = new Vector3(x, 10, x);
    }

    public void Destroy()
    {
        _castingPuzzleData.OnRotatedStage -= SetLenternPosition;
        // TODO
        // 랜턴이 배경으로써 위치에 배치되도록.
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
#if UNITY_EDITOR
        SetSpawnPoint(_castingPuzzleData);
#endif
    }

    private void Lentern_Active(Vector3 spwanPos)
    {
        StartCoroutine(MoveLantern(spwanPos));
    }


    private void SetLenternPosition(Face face)
    {
        _onRotated = true;
    }

    private void SetSpawnPoint(CubePuzzleDataReader puzzleData)
    {
        float _cubeInterval = _cubeWidth + LanternInterval;

        FrontSide = _cubeCenter + new Vector3(_cubeCenter.x, _cubeCenter.y, _cubeCenter.z + _cubeInterval);
        BackSide = _cubeCenter + new Vector3(_cubeCenter.x, _cubeCenter.y, _cubeCenter.z - _cubeInterval);
        LeftSide = _cubeCenter + new Vector3(_cubeCenter.x - _cubeInterval, _cubeCenter.y, _cubeCenter.z);
        RightSide = _cubeCenter + new Vector3(_cubeCenter.x + _cubeInterval, _cubeCenter.y, _cubeCenter.z);
    }

    private IEnumerator MoveLantern(Vector3 spawnPoint)
    {
        this.transform.position = spawnPoint;
        _onRotated = false;

        float t = 0;
        Vector3 current = transform.position;
        Vector3 target = current + new Vector3(0, _cubeWidth, 0) + LanternMoveHight;
        while (t < MoveTime)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(current, target, t);
            yield return null;
        }

        _animator.SetTrigger("Bright");

        while (!_onRotated)
        {
            yield return null;
        }

        yield return new WaitForSeconds(LanternDelayTime);

        t = 0;
        current = transform.position;
        target = current - new Vector3(0, _cubeWidth, 0) - LanternMoveHight;
        while (t < MoveTime)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(current, target, t);
            yield return null;
        }
    }

    public void SetMediator(IMediatorInstance mediator)
    {

    }
    public void InstreamData(byte[] data)
    {
        // SystemReader.IsClearFace(data);
        // SystemReader.CLEAR_LEFT_FACE == data;

        if (SystemReader.CLEAR_TOP_FACE == data || SystemReader.CLEAR_BACK_FACE == data)
        {
            Lentern_Active(LeftSide);
        }
        else
        {
            Lentern_Active(FrontSide);
        }
    }
}
