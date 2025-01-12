using System;
using System.Linq;
using UnityEngine;

namespace Puzzle
{
    [CreateAssetMenu(menuName = "Custom/Puzzle/FlowerPuzzle")]
    public class FlowerPuzzleInstance : ScriptableObject, IInstance, IPuzzleInstance, IDestroyable
    {
        public DataReader DataReader { get; private set; } = new FlowerReader();
        private CubeMap<Flower> _cubeMap;
        private IPresentation _flwerPresentation;
        private readonly HitBoxLink _dataLink = new();
        [SerializeField] private Flower _flowerPrefab;

        public void SetMediator(IMediatorInstance mediator)
        {
            _dataLink.Mediator = mediator;
        }

        public void InstreamData(byte[] data)
        {
            _flwerPresentation.InstreamData(data);
        }

        public void Init(CubePuzzleDataReader puzzleData)
        {
            var instantiator = new Instantiator<Flower>(_flowerPrefab);
            _cubeMap = new CubeMap<Flower>(puzzleData.Width, instantiator);
            _flwerPresentation = new FlowerPresentation(_cubeMap);
            float offset = ((puzzleData.Width) / 2f) - 0.5f;
            float offsetY = (puzzleData.BaseTransformSize.x / 2f) - 0.5f;

            foreach (var index in _cubeMap.GetIndex())
            {
                var face = index[2];
                var flower = _cubeMap.GetElements(index);
                _dataLink.Link(flower, index.Concat(new byte[] { 0 }).ToArray<byte>());
                puzzleData.GetLocation(index, out var position, out var rotation);
                flower.transform.SetParent(puzzleData.BaseTransform);
                flower.transform.localPosition = position;
                flower.transform.localRotation = rotation;
                _flwerPresentation.InstreamData(index.Concat(new byte[] { puzzleData.GetElement(index) }).ToArray<byte>());
            }

        }

        public void Destroy()
        {
            foreach (var obj in _cubeMap.Elements)
            {
                Destroy(obj);
            }
            _cubeMap = null;
        }



    }
}

