using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 코딩 표준
[RequireComponent(typeof(Animator))]
public class MonsterController : MonoBehaviour
{
    private static readonly byte[] ENTER_BOSS = { 10 }; // 입장
    private static readonly byte[] EXIT_BOSS = { 11 }; // 퇴장

    // 필드 변수의 접두사
    // _
    private readonly Queue<byte[]> _commandQueue = new(); // 대기 큐
    private MonsterState _characterState;
    private bool _isActing = false; // 행동 패턴 겹치지 않도록
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void InstreamData(byte[] data)
    {
        _commandQueue.Enqueue(data); // 커맨드 큐에 추가

        if (!_isActing)
        {
            ProcessNextCommand(); // 행동 중이 아니면 다음 커맨드 실행
        }
    }

    private void ProcessNextCommand()
    {
        if (_commandQueue.Count == 0) // 커맨드가 없으면 종료
        {
            _isActing = false;
            return;
        }

        var command = _commandQueue.Dequeue(); // 커맨드 가져오기
        _isActing = true;

        // 바이트 값에 따라 명령 실행
        if (command.Length == 1 && command == ENTER_BOSS)
        {
            if (_characterState == MonsterState.None)
            {
                TrasitionState(MonsterState.Enter);
            }
        }
        else if (command.Length == 1 && command == EXIT_BOSS)
        {
            if (_characterState == MonsterState.None)
            {
                TrasitionState(MonsterState.Die);
            }
        }
        else if (command.Length == 3) // 길이가 3일경우 슬라임 소환 루틴
        {
            if (_characterState == MonsterState.None)
            {
                TrasitionState(MonsterState.Action1);
            }
        }
        else
        {
            Debug.LogWarning("알 수 없는 입력 값");
            _isActing = false;
            ProcessNextCommand();
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
        _isActing = false;
        ProcessNextCommand();
    }

    private void Update()
    {
        if (_commandQueue.Count == 0 ||
            !_isActing) // 커맨드가 없으면 종료
        {
            _isActing = false;
            return;
        }

        ProcessNextCommand();
    }
}

public enum MonsterState // 애니메이터 트리거
{
    None,
    Enter,
    Action1,
    Die
}