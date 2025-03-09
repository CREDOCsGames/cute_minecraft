using Puzzle;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LanternController : MonoBehaviour, IInstance, IReleasable, IPuzzleInstance
{
    public DataReader DataReader { get; private set; } = new SystemReader();
    private CubePuzzleReader _reader;
    private int index;
    [SerializeField] private float duration;
    [SerializeField] private Vector2 h;
    [SerializeField] private List<Transform> _lanternPositions;
    public void InstreamData(byte[] data)
    {
        index = -1;
        if (SystemReader.CLEAR_TOP_FACE.Equals(data))
        {
            index = 0;
        }
        else
        if (SystemReader.CLEAR_LEFT_FACE.Equals(data))
        {
            index = 1;
        }
        else
        if (SystemReader.CLEAR_FRONT_FACE.Equals(data))
        {
            index = 2;
        }
        else
        if (SystemReader.CLEAR_RIGHT_FACE.Equals(data))
        {
            index = 3;
        }
        else
        if (SystemReader.CLEAR_BACK_FACE.Equals(data))
        {
            index = 4;
        }
        else
        if (SystemReader.CLEAR_BOTTOM_FACE.Equals(data))
        {
            return;
        }
        if (0 <= index && index < _lanternPositions.Count)
        {
            CoroutineRunner.instance.StartCoroutine(PlayClearEvent(_lanternPositions[index]));
        }
    }
    private IEnumerator PlayClearEvent(Transform lantern)
    {
        CoroutineRunner.instance.StartCoroutine(DownLantern());
        yield return new WaitForSeconds(duration + 0.5f);
        CoroutineRunner.instance.StartCoroutine(UpLantern(lantern));
    }
    private IEnumerator UpLantern(Transform lantern)
    {
        float t;
        Vector3 prePos, nextPos;
        prePos = lantern.position;
        prePos.y = h.x;
        nextPos = lantern.position;
        nextPos.y = h.y;
        t = 0;
        gameObject.SetActive(true);
        while (t < 1)
        {
            t = Mathf.Clamp01(t + Time.deltaTime / duration);
            transform.position = Vector3.Lerp(prePos, nextPos, t);
            yield return null;
        }
    }
    private IEnumerator DownLantern()
    {
        float t;
        Vector3 prePos, nextPos;
        prePos = transform.position;
        nextPos = transform.position;
        nextPos.y = h.x;
        t = 0;
        while (t < 1)
        {
            t = Mathf.Clamp01(t + Time.deltaTime / duration);
            transform.position = Vector3.Lerp(prePos, nextPos, t);
            yield return null;
        }
        gameObject.SetActive(false);
    }
    public void SetMediator(IMediatorInstance mediator)
    {
    }
    public void DoRelease()
    {
    }

    public void InitInstance(CubePuzzleReader puzzleData)
    {
        _reader = puzzleData;
        _reader.OnRotated += (p, n) => CoroutineRunner.instance.StartCoroutine(DownLantern());
    }
}
