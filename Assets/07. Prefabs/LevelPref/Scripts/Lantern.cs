using Puzzle;
using UnityEditor;
using UnityEngine;

public class Lantern : MonoBehaviour, IInstance, IPuzzleInstance, IDestroyable
{
    public float MoveSpeed;
    public float DelayTime;
    public float LenternInterval;
    public bool IsDisapear { get; private set; } = false;
    public DataReader DataReader { get; private set; } = new SystemReader();
    private bool _apear;
    private bool _bright;
    private float _time;
    private byte _cubeWidth;
    private Vector3 _cubeCenter;
    private Transform _frontSide;
    private Transform _leftSide;
    private Transform _rightSide;
    private Transform _backSide;
    private CubePuzzleDataReader _castingPuzzleData;

    public void Init(CubePuzzleDataReader puzzleData)
    {
        _castingPuzzleData = puzzleData;
        _castingPuzzleData.OnRotatedStage += SetLenternPosition;
        _cubeWidth = puzzleData.Width;
        SetSpawnPoint(_castingPuzzleData);
    }
    public void SetMediator(IMediatorInstance mediator)
    {

    }
    public void Destroy()
    {
        _castingPuzzleData.OnRotatedStage -= SetLenternPosition;
    }
    public void InstreamData(byte[] data)
    {

    }
    private void ActiveLentern(Transform spwanPos)
    {
        transform.position = spwanPos.position;
        gameObject.SetActive(true);
        _apear = true;
        _bright = true;
        _time = 0;
    }
    private Transform SwitchPositionFromDirection(Vector3 direction)
    {
        if (direction == new Vector3(-1, 0, 0))
        {
            return _leftSide;
        }
        else if (direction == new Vector3(1, 0, 0))
        {
            return _rightSide;
        }
        else if (direction == new Vector3(0, 0, -1))
        {
            return _frontSide;
        }
        else if (direction == new Vector3(0, 0, 1))
        {
            return _backSide;
        }
        else
        {
            return null;
        }
    }
    private void SetLenternPosition(Face face)
    {
        if (face == Face.top || face == Face.right || face == Face.bottom)
        {
            ActiveLentern(SwitchPositionFromDirection(-this.transform.forward));
        }
        else
        {
            ActiveLentern(SwitchPositionFromDirection(this.transform.up));
        }
    }
    private void SetSpawnPoint(CubePuzzleDataReader puzzleData)
    {
        _cubeCenter = puzzleData.BaseTransform.position;
        float _cubeInterval = _cubeWidth + LenternInterval;

        _frontSide = new GameObject().transform;
        _backSide = new GameObject().transform;
        _leftSide = new GameObject().transform;
        _rightSide = new GameObject().transform;

        _frontSide.position = _cubeCenter + new Vector3(_cubeCenter.x, _cubeCenter.y, _cubeCenter.z + _cubeInterval);
        _backSide.position = _cubeCenter + new Vector3(_cubeCenter.x, _cubeCenter.y, _cubeCenter.z - _cubeInterval);
        _leftSide.position = _cubeCenter + new Vector3(_cubeCenter.x - _cubeInterval, _cubeCenter.y, _cubeCenter.z);
        _rightSide.position = _cubeCenter + new Vector3(_cubeCenter.x + _cubeInterval, _cubeCenter.y, _cubeCenter.z);
    }
    private void Update()
    {
        if (_apear)
        {
            if (this.transform.position.y < 2.5f)
            {
                this.transform.position += new Vector3(0, MoveSpeed, 0);
            }
            else if (this.transform.position.y < 2.8f)
            {
                this.transform.position += new Vector3(0, MoveSpeed * 0.5f, 0);
            }
            else
            {
                _time += Time.deltaTime;
                if (_time > DelayTime)
                {
                    _apear = false;
                }
            }
        }

        if (!_apear && _bright)
        {
            GetComponent<Animator>().SetTrigger("Bright");
            _bright = false;
        }

        if (IsDisapear)
        {
            if (this.transform.position.y > -3.5f)
            {
                this.transform.position += new Vector3(0, -MoveSpeed, 0);
            }
            else
            {
                IsDisapear = false;
                this.gameObject.SetActive(false);
            }
        }
    }

#if false
    public void InstreamData(byte[] data)
    {
        if (data == SystemReader.CLEAR_TOP_FACE) // Ŭ����.
        {
            Lentern_Active(RightSide);
        }
        else
        if (data == SystemReader.CLEAR_RIGHT_FACE)
        {
            Lentern_Active(RightSide);
        }
        else
        if (data == SystemReader.CLEAR_BOTTOM_FACE)
        {
            Lentern_Active(FrontSide);
        }
        else
        if (data == SystemReader.CLEAR_FRONT_FACE)
        {
            Lentern_Active(LeftSide);
        }
        else
        if (data == SystemReader.CLEAR_LEFT_FACE)
        {
            Lentern_Active(BackSide);
        }
        else
        if (data == SystemReader.CLEAR_BACK_FACE)
        {
            // Boss Todo
            _enterBossStage = true;
            SetSpawnPoint(_castingPuzzleData);
        }
        else
        if (data == SystemReader.ROTATE_CUBE) // need => Rotation Data
        {
            IsDisapear = true;
        }
        else
        if (data == null) // need => Fall Data
        {
            // Boss Todo
            SetSpawnPoint(_castingPuzzleData);
        }

    }
    

#endif
}
