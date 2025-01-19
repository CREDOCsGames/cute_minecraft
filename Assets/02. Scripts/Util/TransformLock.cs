using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformLock : MonoBehaviour
{
    private Vector3 _firstPosition;
    private Vector3 _firstRotation;
    private Vector3 _firstScale;
    [SerializeField] private bool _lockPosX;
    [SerializeField] private bool _lockPosY;
    [SerializeField] private bool _lockPosZ;
    [SerializeField] private bool _lockRotX;
    [SerializeField] private bool _lockRotY;
    [SerializeField] private bool _lockRotZ;
    [SerializeField] private bool _lockScaX;
    [SerializeField] private bool _lockScaY;
    [SerializeField] private bool _lockScaZ;
    private void Awake()
    {
        _firstPosition = transform.position;
        _firstRotation = transform.eulerAngles;
        _firstScale = transform.localScale;
    }
    private void FixedUpdate()
    {
        Vector3 lockPosition = _firstPosition;
        if (!_lockPosX)
        {
            lockPosition.x = transform.position.x;
        }
        if (!_lockPosY)
        {
            lockPosition.y = transform.position.y;
        }
        if (!_lockPosZ)
        {
            lockPosition.z = transform.position.z;
        }
        transform.position = lockPosition;

        Vector3 lockRotation = _firstRotation;
        if (!_lockRotX)
        {
            lockRotation.x = transform.eulerAngles.x;
        }
        if(!_lockRotY)
        {
            lockRotation.y = transform.eulerAngles.y;
        }
        if (!_lockRotZ)
        {
            lockRotation.z = transform.eulerAngles.z;
        }
        transform.eulerAngles = lockRotation;

        Vector3 lockScale = _firstScale;
        if (!_lockScaX)
        {
            lockScale.x = transform.localScale.x;
        }
        if (!_lockScaY)
        {
            lockScale.y = transform.localScale.y;
        }
        if (!_lockScaZ)
        {
            lockScale.z = transform.localScale.z;
        }
        transform.localScale = lockScale;
    }
}
