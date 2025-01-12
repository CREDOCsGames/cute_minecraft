using Controller;
using Movement;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static Movement.MovementAction;

namespace Puzzle
{

    public class CubePresentation : IPresentation
    {
        private CubePuzzleDataReader _reader;

        private readonly MovementComponent _movement;
        public CubePresentation(MovementComponent movementComponent)
        {
            _movement = movementComponent;
        }
        public void SetR(CubePuzzleDataReader puzzleData)
        {
            _reader = puzzleData;
        }
        public void InstreamData(byte[] data)
        {
            if (SystemReader.CLEAR_TOP_FACE.Equals(data))
            {
                Command(ROTATE_ROLL_P90);
            }
            else if (SystemReader.CLEAR_LEFT_FACE.Equals(data))
            {
                Command(ROTATE_PITCH_P90);
            }
            else if (SystemReader.CLEAR_FRONT_FACE.Equals(data))
            {
                Command(ROTATE_PITCH_P90);
            }
            else if (SystemReader.CLEAR_RIGHT_FACE.Equals(data))
            {
                Command(ROTATE_PITCH_P90);
            }
            else if (SystemReader.CLEAR_BACK_FACE.Equals(data))
            {
                Command(ROTATE_ROLL_P90);
            }
            else if (SystemReader.CLEAR_BOTTOM_FACE.Equals(data))
            {
                // Level Clear Event
            }

        }

        public void Command(string path)
        {
            Util.CoroutineRunner.Instance.StartCoroutine(Action(path));
        }

        private IEnumerator Action(string path)
        {
            var character = GameObject.FindAnyObjectByType<CharacterComponent>()._character;
            while (character != null && character.State is not Controller.CharacterState.Idle)
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

            float time = Time.time + 1f;
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
            while (Time.time < time)
            {
                yield return null;
            }
            _reader.Throw.Enable = false;

        }

    }
    public class CubeInstance : MonoBehaviour, IInstance, IPuzzleInstance
    {
        public DataReader DataReader => new SystemReader();
        private AreaWall _areaWall;
        private MovementComponent _movement;
        private CubePresentation _presentation;

        public void Init(CubePuzzleDataReader puzzleData)
        {
            if (!puzzleData.BaseTransform.TryGetComponent(out _movement))
            {
                _movement = puzzleData.BaseTransform.AddComponent<MovementComponent>();
                puzzleData.BaseTransform.GetComponent<Rigidbody>().isKinematic = true;
            }

            {
                Side side = Side.right | Side.left | Side.forward | Side.backward | Side.up;
                Bounds bounds = new Bounds();
                bounds.extents = puzzleData.BaseTransformSize / 2f;
                bounds.center = puzzleData.BaseTransform.position + Vector3.up * puzzleData.BaseTransformSize.y;
                _areaWall = new AreaWall(side, bounds, $"Objects/{AreaWallComponent.Type.Wall}");
                _areaWall.Create();
            }

            {
                Side side = Side.down;
                Bounds bounds = new();
                bounds.extents = puzzleData.BaseTransformSize / 2f;
                bounds.center = puzzleData.BaseTransform.position + Vector3.up * puzzleData.BaseTransformSize.y;
                var wall = new AreaWall(side, bounds, $"Objects/{AreaWallComponent.Type.Wall}");
                wall.Create();
            }
            _presentation = new CubePresentation(_movement);
            _presentation.SetR(puzzleData);
        }

        public void InstreamData(byte[] data)
        {
            _presentation.InstreamData(data);
            if (data.Equals(SystemReader.CLEAR_BACK_FACE))
            {
                _areaWall.SetWall($"Objects/{AreaWallComponent.Type.JumpWall}");
                _areaWall.Destroy();
                _areaWall.Create();
            }
        }

        public void SetMediator(IMediatorInstance mediator)
        {
        }

        public void TurnRight()
        {
            _presentation.Command(ROTATE_ROLL_M90);
        }
        public void TurnLeft()
        {
            _presentation.Command(ROTATE_ROLL_P90);
        }
        public void TurnFront()
        {
            _presentation.Command(ROTATE_PITCH_M90);
        }
        public void TurnBack()
        {
            _presentation.Command(ROTATE_PITCH_P90);
        }
    }

}
