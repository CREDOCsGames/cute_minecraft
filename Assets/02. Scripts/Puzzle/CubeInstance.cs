using Movement;
using Unity.VisualScripting;
using UnityEngine;
using static Movement.MovementAction;

namespace Puzzle
{
    public class CubeInstance : MonoBehaviour, IInstance, IPuzzleInstance, IDestroyable
    {
        public DataReader DataReader => new SystemReader();
        private bool _boss;
        private AreaWall _areaWall;
        private MovementComponent _movement;
        private CubePresentation _presentation;
        private IMediatorInstance _mediator;
        public void Init(CubePuzzleReader reader)
        {
            if (!reader.BaseTransform.TryGetComponent(out _movement))
            {
                _movement = reader.BaseTransform.AddComponent<MovementComponent>();
                reader.BaseTransform.GetComponent<Rigidbody>().isKinematic = true;
            }

            {
                Side side = Side.right | Side.left | Side.forward | Side.backward | Side.up;
                Bounds bounds = new Bounds();
                bounds.extents = reader.BaseTransformSize / 2f;
                bounds.center = reader.BaseTransform.position + Vector3.up * reader.BaseTransformSize.y;
                _areaWall = new AreaWall(side, bounds, $"Objects/{AreaWallComponent.Type.Wall}");
                _areaWall.Create();
            }

            {
                Side side = Side.down;
                Bounds bounds = new();
                bounds.extents = reader.BaseTransformSize / 2f;
                bounds.center = reader.BaseTransform.position + Vector3.up * reader.BaseTransformSize.y;
                var wall = new AreaWall(side, bounds, $"Objects/{AreaWallComponent.Type.Wall}");
                wall.Create();
            }
            _presentation = new CubePresentation(_movement);
            _presentation.SetReader(reader);
            _presentation.RotatedEvent += OnRotated;
        }
        public void InstreamData(byte[] data)
        {
            _presentation.InstreamData(data);
            if (SystemReader.CLEAR_BACK_FACE.Equals(data))
            {
                _boss = true;
                _areaWall.Destroy();
                _areaWall.SetSide(Side.left | Side.forward | Side.backward | Side.right);
                _areaWall.SetWall($"Objects/{AreaWallComponent.Type.JumpWall}");
                _areaWall.Create();
            }
            else if (SystemReader.CLEAR_TOP_FACE.Equals(data))
            {
                _areaWall.Destroy();
                _areaWall.SetSide(Side.left | Side.forward | Side.backward);
                _areaWall.SetWall($"Objects/{AreaWallComponent.Type.Wall}");
                _areaWall.Create();
                _areaWall.SetSide(Side.right);
                _areaWall.SetWall($"Objects/{AreaWallComponent.Type.JumpWall}");
                _areaWall.Create();
            }
            else if (SystemReader.CLEAR_RIGHT_FACE.Equals(data) || SystemReader.CLEAR_FRONT_FACE.Equals(data) || SystemReader.CLEAR_LEFT_FACE.Equals(data))
            {
                _areaWall.Destroy();
                _areaWall.SetSide(Side.left | Side.right | Side.backward);
                _areaWall.SetWall($"Objects/{AreaWallComponent.Type.Wall}");
                _areaWall.Create();
                _areaWall.SetSide(Side.forward);
                _areaWall.SetWall($"Objects/{AreaWallComponent.Type.JumpWall}");
                _areaWall.Create();
            }
            else if (SystemReader.CLEAR_BOTTOM_FACE.Equals(data))
            {
                _areaWall.Destroy();
                _areaWall.SetSide(Side.left | Side.right | Side.forward | Side.backward);
                _areaWall.SetWall($"Objects/{AreaWallComponent.Type.Wall}");
                _areaWall.Create();
            }
        }
        public void TurnRight()
        {
            _presentation.CommandAction(ROTATE_ROLL_M90);
        }
        public void TurnLeft()
        {
            _presentation.CommandAction(ROTATE_ROLL_P90);
        }
        public void TurnFront()
        {
            _presentation.CommandAction(ROTATE_PITCH_M90);
        }
        public void TurnBack()
        {
            _presentation.CommandAction(ROTATE_PITCH_P90);
        }
        public void SetMediator(IMediatorInstance mediator)
        {
            _mediator = mediator;
        }
        private void OnRotated()
        {
            _mediator.InstreamDataInstance<SystemReader>(SystemReader.ROTATE_CUBE);
            if (!_boss)
            {
                _areaWall.Destroy();
                _areaWall.SetWall($"Objects/{AreaWallComponent.Type.Wall}");
                _areaWall.SetSide(Side.left | Side.right | Side.forward | Side.backward);
                _areaWall.Create();
            }
        }
        public void Destroy()
        {
            _presentation.RotatedEvent -= OnRotated;
        }
    }
}
