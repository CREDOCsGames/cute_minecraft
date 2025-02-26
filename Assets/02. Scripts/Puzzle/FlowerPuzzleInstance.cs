using System.Linq;
using UnityEngine;

namespace Puzzle
{
    [CreateAssetMenu(menuName = "Custom/Puzzle/FlowerPuzzle")]
    public class FlowerPuzzleInstance : ScriptableObject, IInstance, IPuzzleInstance, IDestroyable
    {
        public DataReader DataReader { get; private set; } = FlowerReader.Instance;
        private CubeMap<Flower> _cubeMap;
        private readonly HitBoxLink _dataLink = new();
        private CubePuzzleReader _puzzleData;
        [SerializeField] private Flower _flowerPrefab;

        public void Init(CubePuzzleReader puzzleData)
        {
            _puzzleData = puzzleData;
            var instantiator = new Instantiator<Flower>(_flowerPrefab);
            _cubeMap = new CubeMap<Flower>(_puzzleData.Width, instantiator);
            _dataLink.Link(_cubeMap);
            _puzzleData.OnRotatedStage += OnRotated;
            InitFlower();
        }
        public void InitFlower()
        {
            foreach (var index in _cubeMap.GetIndex())
            {
                var flower = _cubeMap.GetElements(index);
                flower.transform.SetParent(_puzzleData.BaseTransform);
                _puzzleData.GetPositionAndRotation(index, out var position, out var rotation);
                flower.transform.SetLocalPositionAndRotation(position, rotation);
                InstreamData(index.Concat(new byte[] { _puzzleData.GetElement(index) }).ToArray<byte>());
            }
        }
        public void InstreamData(byte[] data)
        {
            var face = (Face)data[2];
            if (face is Face.bottom)
            {
                return;
            }
            var flower = _cubeMap.GetElements(data[0], data[1], data[2]);
            flower.gameObject.SetActive(true);
            switch (data[3])
            {
                case (byte)Flower.Type.Red:
                    flower.Color = new Color(191f / 255f, 12f / 255f, 255f / 255f);
                    break;
                case (byte)Flower.Type.Green:
                    flower.Color = Color.cyan;
                    break;
                default:
                    flower.gameObject.SetActive(false);
                    break;
            }
        }
        private void OnRotated(Face nextFace)
        {
            if (nextFace is Face.bottom)
            {
                return;
            }
            foreach (var index in _cubeMap.GetIndex())
            {
                var flower = _cubeMap.GetElements(index);
                var face = index[2];
                flower.gameObject.SetActive(false);
                if ((byte)nextFace == face)
                {
                    index[2] = (byte)Face.top;
                    _puzzleData.GetPositionAndRotation(index, out var position, out var rotation);
                    flower.transform.position = _puzzleData.BaseTransform.position + position;
                    flower.transform.rotation = Quaternion.identity;
                    flower.gameObject.SetActive(flower.Color != Color.clear);
                }
            }
        }
        public void SetMediator(IMediatorInstance mediator)
        {
            _dataLink.Mediator = mediator;
        }
        public void Destroy()
        {
            foreach (var obj in _cubeMap.Elements)
            {
                Destroy(obj);
            }
            _cubeMap = null;
            _puzzleData.OnRotatedStage -= OnRotated;
        }
    }
}

