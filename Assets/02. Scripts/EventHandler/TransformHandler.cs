using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformHandler : MonoBehaviour
{
    Transform mTransform;

    public void SetLocalPositionX(float x)
    {
        var vector = transform.localPosition;
        vector.x = x;
        transform.localPosition = vector;
    }
    public void SetLocalPositionY(float y)
    {
        var vector = transform.localPosition;
        vector.y = y;
        transform.localPosition = vector;
    }
    public void SetLocalPositionZ(float z)
    {
        var vector = transform.localPosition;
        vector.z = z;
        transform.localPosition = vector;
    }
    public void SetLocalRotationX(float x)
    {
        var vector = transform.eulerAngles;
        vector.x = x;
        transform.localEulerAngles = vector;
    }
    public void SetLocalRotationY(float y)
    {
        var vector = transform.eulerAngles;
        vector.y = y;
        transform.localEulerAngles = vector;
    }
    public void SetLocalRotationZ(float z)
    {
        var vector = transform.eulerAngles;
        vector.z = z;
        transform.localEulerAngles = vector;
    }
    public void DefaultPosition()
    {
        transform.localPosition = Vector3.zero;
    }
    public void DefaultRotation()
    {
        transform.localRotation = Quaternion.identity;
    }
    public void SetLocalRotation(Vector3 rot)
    {
        transform.eulerAngles = rot;
    }

    void Awake()
    {
        mTransform = GetComponent<Transform>();
    }
}
