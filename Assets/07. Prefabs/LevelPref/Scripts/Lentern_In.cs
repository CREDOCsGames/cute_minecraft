using Puzzle;
using UnityEngine;

public class Lentern_In : MonoBehaviour, IInstance, IPuzzleInstance
{
    public DataReader DataReader { get; private set; } = new SystemReader();

    public float MoveSpeed;
    public float DelayTime;
    public float LenternInterval;

    public Transform FrontSide;
    public Transform LeftSide;
    public Transform RightSide;
    public Transform BackSide;

    private Vector3 _cubeCenter;
    private byte _cubeWidth;
    private bool _bright = false;
    private bool _apear = false;
    private bool _disapear = false;
    private float _time;

    public void InstreamData(byte[] data)
    {
        if(data == SystemReader.CLEAR_TOP_FACE) // Å¬¸®¾î.
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
            // Todo
        }
        else
        if (data == null) // need => Rotation Data
        {
            _disapear = true;
        }

    }

    public void SetMediator(IMediatorInstance mediator)
    {
        
    }

    public void Init(CubeMapReader puzzleData)
    {
        _cubeCenter = puzzleData.BaseTransform.position;
        _cubeWidth = puzzleData.Width;
        float _cubeInterval = _cubeWidth + LenternInterval;

        FrontSide.position = _cubeCenter + new Vector3(_cubeCenter.x, _cubeCenter.y , _cubeCenter.z + _cubeInterval);
        BackSide.position = _cubeCenter + new Vector3(_cubeCenter.x, _cubeCenter.y , _cubeCenter.z - _cubeInterval);
        LeftSide.position = _cubeCenter + new Vector3(_cubeCenter.x - _cubeInterval, _cubeCenter.y, _cubeCenter.z);
        RightSide.position = _cubeCenter + new Vector3(_cubeCenter.x + _cubeInterval, _cubeCenter.y, _cubeCenter.z);
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
                if(_time > DelayTime)
                {
                    _apear = false;
                }
            }
        }

        if(!_apear && _bright)
        {
            GetComponent<Animator>().SetTrigger("Bright");
            _bright = false;
        }

        if (_disapear)
        {
            if (this.transform.position.y > -3.5f)
            {
                this.transform.position += new Vector3(0, -MoveSpeed, 0);
            }
            else
            {
                _disapear = false;
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

    
}
