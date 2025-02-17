using Controller;
using Movement;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static Movement.MovementAction;

namespace Puzzle
{
    public class CubePresentation : IPresentation
    {
        public event Action RotatedEvent;
        public bool IsActing => Time.time < _coolTime;
        private float _coolTime;
        private CubePuzzleReader _reader;
        private readonly MovementComponent _movement;
        public CubePresentation(MovementComponent movementComponent)
        {
            _movement = movementComponent;
        }
        public void SetReader(CubePuzzleReader reader)
        {
            _reader = reader;
        }
        public void InstreamData(byte[] data)
        {
        }
        public void CommandAction(string path)
        {
            if (IsActing)
            {
                return;
            }
            CoroutineRunner.instance.StartCoroutine(Action(path));
        }
        private IEnumerator Action(string path)
        {
            var character = GameObject.FindAnyObjectByType<CharacterComponent>()._character;
            while (character != null && character.State is not CharacterState.Idle)
            {
                yield return null;
            }
            character.ChangeController(new CanNotControl());
            yield return new WaitForSeconds(0.1f);
            character.Jump();
            character?.ChangeController(new JumpState());

            if (MovementAction.TryGetAction(path, out var action))
            {
                _movement.PlayMovement(action);
            }
            if (_reader.Throw == null)
            {
                yield break;
            }

            _coolTime = Time.time + 1f;
            yield return new WaitForSeconds(0.2f);
            _reader.Throw.Enable = true;
            if (path.Equals(ROTATE_ROLL_M90))
            {
                _reader.Throw.Dir = Vector3.left + Vector3.up;
            }
            else if (path.Equals(ROTATE_ROLL_P90))
            {
                _reader.Throw.Dir = Vector3.right + Vector3.up;
            }
            else if (path.Equals(ROTATE_PITCH_P90))
            {
                _reader.Throw.Dir = Vector3.back + Vector3.up;
            }
            else
            {
                _reader.Throw.Dir = Vector3.forward + Vector3.up;
            }
            while (IsActing)
            {
                yield return null;
            }
            _reader.Throw.Enable = false;
            RotatedEvent?.Invoke();
        }
    }

}
