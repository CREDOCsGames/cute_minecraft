using Puzzle;
using UnityEngine;

public class Lantern_In : MonoBehaviour, IInstance, IPuzzleInstance
{
    public DataReader DataReader { get; private set; } = new SystemReader();

    public float MoveSpeed;
    public float DelayTime;

    public Transform FrontSide;
    public Transform LeftSide;
    public Transform RightSide;
    public Transform BackSide;
    public bool IsDisapear { get; private set; } = false;

    private bool _apear = false;
    private bool _bright = false;
    private float _time;
    private byte _width;
    private Vector3 _center;

    public void InstreamData(byte[] data)
    {
        if (data == SystemReader.CLEAR_TOP_FACE) // Å¬¸®¾î.
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
            IsDisapear = true;
        }

    }

    public void Init(CubePuzzleDataReader puzzleData)
    {
        _center = puzzleData.BaseTransform.position;
        _width = puzzleData.Width;
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

}
