using Puzzle;
using UnityEngine;
using UnityEngine.UIElements;

public class Lantern_In : MonoBehaviour, IInstance, IPuzzleInstance, IDestroyable
{
    public DataReader DataReader { get; private set; } = new SystemReader();

    public float MoveSpeed;
    public float DelayTime;
    public float LenternInterval;

    public Transform FrontSide;
    public Transform LeftSide;
    public Transform RightSide;
    public Transform BackSide;
    public bool IsDisapear { get; private set; } = false;

    private CubePuzzleDataReader _castingPuzzleData;
    private bool _apear = false;
    private bool _bright = false;
    private float _time;
    private Vector3 _cubeCenter;
    private byte _cubeWidth;


    public void Init(CubePuzzleDataReader puzzleData)
    {
        _castingPuzzleData = puzzleData;
        _castingPuzzleData.OnRotatedStage += SetLenternPosition;
        _cubeWidth = puzzleData.Width;
        SetSpawnPoint(_castingPuzzleData);
    }

    public void Destroy()
    {
        _castingPuzzleData.OnRotatedStage -= SetLenternPosition;
    }

    public void SetMediator(IMediatorInstance mediator)
    {

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

    private void Lentern_Active(Transform spwanPos)
    {
        this.gameObject.transform.position = spwanPos.position;
        this.gameObject.SetActive(true);
        _apear = true;
        _bright = true;
        _time = 0;
    }

    private Transform SwitchPositionFromDirection(Vector3 direction)
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
            return null;
        }
    }

    private void SetLenternPosition(Face face)
    {
        if (face == Face.top || face == Face.right || face == Face.bottom)
        {
            Lentern_Active(SwitchPositionFromDirection(-this.transform.forward));
        }
        else
        {
            Lentern_Active(SwitchPositionFromDirection(this.transform.up));
        }
    }

    public void InstreamData(byte[] data)
    {

    }

    private void SetSpawnPoint(CubePuzzleDataReader puzzleData)
    {
        _cubeCenter = puzzleData.BaseTransform.position;
        float _cubeInterval = _cubeWidth + LenternInterval;

        FrontSide.position = _cubeCenter + new Vector3(_cubeCenter.x, _cubeCenter.y, _cubeCenter.z + _cubeInterval);
        BackSide.position = _cubeCenter + new Vector3(_cubeCenter.x, _cubeCenter.y, _cubeCenter.z - _cubeInterval);
        LeftSide.position = _cubeCenter + new Vector3(_cubeCenter.x - _cubeInterval, _cubeCenter.y, _cubeCenter.z);
        RightSide.position = _cubeCenter + new Vector3(_cubeCenter.x + _cubeInterval, _cubeCenter.y, _cubeCenter.z);
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
