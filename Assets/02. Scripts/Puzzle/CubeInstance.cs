using Controller;
using Movement;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Puzzle
{

    public class CubePresentation : IPresentation
    {
        private const string ROTATE_RIGTH = "MovementAction/RotateRight";
        private const string ROTATE_LEFT = "MovementAction/RotateLeft";
        private const string ROTATE_BWD = "MovementAction/RotateBackward";
        private const string ROTATE_FWD = "MovementAction/RotateForward";

        private readonly MovementComponent _movement;
        public CubePresentation(MovementComponent movementComponent)
        {
            _movement = movementComponent;
        }
        public void InstreamData(byte[] data)
        {
            if (SystemReader.CLEAR_RIGHT_FACE.Equals(data))
            {
                Command(ROTATE_LEFT);
            }
            else if (SystemReader.CLEAR_LEFT_FACE.Equals(data))
            {
                Command(ROTATE_LEFT);
            }
            else if (SystemReader.CLEAR_FRONT_FACE.Equals(data))
            {
                Command(ROTATE_LEFT);
            }
            else if (SystemReader.CLEAR_BACK_FACE.Equals(data))
            {
                Command(ROTATE_BWD);
            }
            else if (SystemReader.CLEAR_TOP_FACE.Equals(data))
            {
                Command(ROTATE_BWD);
            }
            else if (SystemReader.CLEAR_BOTTOM_FACE.Equals(data))
            {
                Command(ROTATE_LEFT);
            }

        }

        private void Command(string path)
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
            yield return new WaitForSeconds(1f);
            var action = Resources.Load<MovementAction>(path);
            character.Jump();
            character?.ChangeController(new JumpState());

            if (action != null)
            {
                _movement.PlayMovement(action);
            }
        }

    }
    [CreateAssetMenu(menuName = "Custom/CubeInstance")]
    public class CubeInstance : ScriptableObject, IInstance, IPuzzleInstance
    {
        public DataReader DataReader => new SystemReader();
        private AreaWall _areaWall;
        private MovementComponent _movement;
        private IPresentation _presentation;

        public void Init(CubeMapReader puzzleData)
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
                Bounds bounds = new Bounds();
                bounds.extents = puzzleData.BaseTransformSize / 2f;
                bounds.center = puzzleData.BaseTransform.position + Vector3.up * puzzleData.BaseTransformSize.y;
                var wall = new AreaWall(side, bounds, $"Objects/{AreaWallComponent.Type.Wall}");
                wall.Create();
            }
            _presentation = new CubePresentation(_movement);
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

    }

}
