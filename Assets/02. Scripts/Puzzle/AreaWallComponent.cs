using UnityEngine;

namespace Puzzle
{
    public class AreaWallComponent : MonoBehaviour, IPuzzleInstance, IInstance
    {
        public enum Type
        {
            Wall,
            JumpWall
        }

        private AreaWall _wall;
        [SerializeField] private Side _side;
        [SerializeField] private Type _wallType;
        private Bounds _bounds;

        public DataReader DataReader => new SystemReader();

        public event System.Action<byte[]> InstreamEvent;

        public void Init(CubePuzzleDataReader puzzleData)
        {
            _bounds = new Bounds();
            _bounds.extents = puzzleData.BaseTransformSize / 2f;
            _bounds.center = puzzleData.BaseTransform.position + Vector3.up * puzzleData.BaseTransformSize.y;
            _wall = new AreaWall(_side, _bounds, $"Objects/{_wallType}");
            MakeWall($"Objects/{_wallType}");
        }
        public void InstreamData(byte[] data)
        {
            if (data.Equals(SystemReader.CLEAR_BACK_FACE))
            {
                MakeWall($"Objects/{Type.JumpWall}");
            }
        }
        private void MakeWall(string wall)
        {
            _wall.Destroy();
            _wall.SetWall(wall);
            _wall.Create();
        }

        public void SetMediator(IMediatorInstance mediator)
        {
        }
    }
}