using NW;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 코딩 표준
[RequireComponent(typeof(Animator))]
public class MonsterController : MonoBehaviour, NW.IInstance
{
    public DataReader DataReader => new MonsterReader();
    private Animator _animator;
    private MonsterState _characterState;
    private readonly Queue<byte[]> _commandQueue = new();

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void InstreamData(byte[] data)
    {
        if (data == MonsterReader.BOSS_EXIT)
        {
            _commandQueue.Clear();
        }
        if (_characterState is not MonsterState.None &&
            data == MonsterReader.BOSS_SPAWN)
        {
            Debug.LogWarning($"Boss's status is already {_characterState.ToString()}.");
        }
        _commandQueue.Enqueue(data);
    }

    private void ProcessNextCommand()
    {
        var command = _commandQueue.Dequeue();

        if (command == MonsterReader.BOSS_SPAWN &&
            _characterState is MonsterState.None)
        {
            TrasitionState(MonsterState.Enter);
            return;
        }

        if (_characterState is not MonsterState.Enter)
        {
            return;
        }
        if (command == MonsterReader.BOSS_EXIT)
        {
            TrasitionState(MonsterState.Die);
            return;
        }
        if (command == MonsterReader.BOSS_SPIT_OUT)
        {
            TrasitionState(MonsterState.Action1);
            return;
        }
    }

    // 애니메이션 키 이벤트
    public void SetState(MonsterState state)
    {
        _characterState = state;
    }

    private void TrasitionState(MonsterState state)
    {
        _characterState = state;
        _animator.SetTrigger(_characterState.ToString());
    }

    private void Update()
    {
        if (_commandQueue.Count == 0) // 커맨드가 없으면 종료
        {
            return;
        }

        ProcessNextCommand();
    }

    public void SetMediator(IMediatorInstance mediator)
    {
    }
}

public enum MonsterState // 애니메이터 트리거
{
    None,
    Enter,
    Action1,
    Die
}