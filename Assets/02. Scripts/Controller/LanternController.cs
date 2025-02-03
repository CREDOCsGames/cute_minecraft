using Puzzle;
using System.Collections;
using UnityEngine;

public class LanternController : MonoBehaviour, IInstance, IPuzzleInstance, IDestroyable
{
    public DataReader DataReader { get; private set; } = new SystemReader();

    public float MoveTime;
    public float LenternInterval;
    public Vector3 LanternMoveHight;

    public Vector3 FrontSide;
    public Vector3 LeftSide;
    public Vector3 RightSide;
    public Vector3 BackSide;
    public bool IsDisapear { get; private set; } = false;

    private CubePuzzleDataReader _castingPuzzleData;
    private Transform _cube;
    private Vector3 _cubeCenter;
    private byte _cubeWidth;

    public void Init(CubePuzzleDataReader puzzleData)
    {
        this.gameObject.SetActive(true);
        _castingPuzzleData = puzzleData;
        _castingPuzzleData.OnRotatedStage += SetLenternPosition;
        _cubeWidth = puzzleData.Width;
        SetSpawnPoint(_castingPuzzleData);
    }

    public void Destroy()
    {
        _castingPuzzleData.OnRotatedStage -= SetLenternPosition;
    }

    private void Update()
    {
        
    }

    private void Lentern_Active(Vector3 spwanPos)
    {
        StartCoroutine(MoveLantern(spwanPos));
    }

    private Vector3 SwitchPositionFromDirection(Vector3 direction)
    {
        if (direction == new Vector3(-1, 0, 0))
        {
            return LeftSide;
        }
        else if (direction == new Vector3(1, 0, 0))
        {
            return RightSide;
        }
        else if (direction == new Vector3(0, 0, -1))
        {
            return FrontSide;
        }
        else if (direction == new Vector3(0, 0, 1))
        {
            return BackSide;
        }
        else
        {
            return new Vector3();
        }
    }

    private void SetLenternPosition(Face face)
    {
        if (face == Face.top || face == Face.right || face == Face.bottom)
        {
            Lentern_Active(SwitchPositionFromDirection(-_cube.transform.forward));
        }
        else
        {
            Lentern_Active(SwitchPositionFromDirection(_cube.transform.up));
        }
    }

    private void SetSpawnPoint(CubePuzzleDataReader puzzleData)
    {
        _cubeCenter = puzzleData.BaseTransform.position;
        _cube = puzzleData.BaseTransform;
        float _cubeInterval = _cubeWidth + LenternInterval;

        FrontSide = _cubeCenter + new Vector3(_cubeCenter.x, _cubeCenter.y, _cubeCenter.z + _cubeInterval);
        BackSide = _cubeCenter + new Vector3(_cubeCenter.x, _cubeCenter.y, _cubeCenter.z - _cubeInterval);
        LeftSide = _cubeCenter + new Vector3(_cubeCenter.x - _cubeInterval, _cubeCenter.y, _cubeCenter.z);
        RightSide = _cubeCenter + new Vector3(_cubeCenter.x + _cubeInterval, _cubeCenter.y, _cubeCenter.z);
    }

    private IEnumerator MoveLantern(Vector3 spawnPoint)
    {
        float t = 0;
        Vector3 current = transform.position;
        Vector3 target = current - LanternMoveHight;
        while (t < MoveTime)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(current, target, t);
            yield return null;
        }

        this.transform.position = spawnPoint;

        t = 0;
        current = transform.position;
        target = current + LanternMoveHight;
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

    }
}
