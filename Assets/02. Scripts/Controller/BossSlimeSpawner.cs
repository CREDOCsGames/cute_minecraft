using Battle;
using Controller;
using System;
using UnityEngine;

public class BossSlimeSpawner : MonoBehaviour
{
    public MonsterComponent slimePrefab;
    private MonsterComponent _slimeInstance;
    private readonly JumpController _controller = new();
    [SerializeField] private Transform _bitePoint;
    [SerializeField] private AnimationCurve _jumpCurve;
    public event Action OnFailed;


    public void SpawnAt(Vector3 slimePosition)
    {
        _slimeInstance = Instantiate(slimePrefab, slimePosition, Quaternion.identity);
        _controller.StartPoint = _bitePoint.position;
        _controller.EndPoint = slimePosition;
        _controller.JumpCurve = _jumpCurve;
        StartCoroutine(_controller.Move(_slimeInstance.transform));
        _slimeInstance._character.OnChagedState += CheckFailed;
    }

    public void CheckFailed(CharacterState state)
    {
        if(_slimeInstance == null)
        {
            return;
        }
        if(state is CharacterState.Hit)
        {
            _controller.Stop();
        }
        _slimeInstance._character.OnChagedState -= CheckFailed;
        OnFailed?.Invoke();
    }
    public void OnSuccess()
    {
        if (_slimeInstance == null)
        {
            return;
        }
        _slimeInstance._character.OnChagedState -= CheckFailed;
        _slimeInstance._character.Die();
    }
}