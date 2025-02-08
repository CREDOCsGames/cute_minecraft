using Puzzle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternController : MonoBehaviour, IInstance, IDestroyable
{
    [SerializeField] private float duration;
    [SerializeField] private Vector2 h;
    [SerializeField] private List<Transform> _lanternPositions;
    public DataReader DataReader { get; private set; } = new SystemReader();

    public void InstreamData(byte[] data)
    {
        int index = -1;
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
        if (index < 0 || _lanternPositions.Count <= index)
        {
            return;
        }
        StartCoroutine(PlayClearEvent(_lanternPositions[index]));
    }

    private IEnumerator PlayClearEvent(Transform lantern)
    {
        float t;
        Vector3 prePos, nextPos;

        // down
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

        yield return new WaitForSeconds(0.5f);

        // up
        prePos = lantern.position;
        prePos.y = h.x;
        nextPos = lantern.position;
        nextPos.y = h.y;
        t = 0;
        while (t < 1)
        {
            t = Mathf.Clamp01(t + Time.deltaTime / duration);
            transform.position = Vector3.Lerp(prePos, nextPos, t);
            yield return null;
        }
    }
    public void SetMediator(IMediatorInstance mediator)
    {
    }
    public void Destroy()
    {
    }
}
