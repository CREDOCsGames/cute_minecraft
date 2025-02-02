using System;
using System.Linq;
using UnityEngine;
using Util;

namespace Puzzle
{
    [CreateAssetMenu(menuName = "Custom/Puzzle/FlowerPuzzle")]
    public class FlowerPuzzleInstance : ScriptableObject, IInstance, IPuzzleInstance, IDestroyable
    {
        public DataReader DataReader { get; private set; } = new FlowerReader();
        private CubeMap<Flower> _cubeMap;
        private IPresentation _flwerPresentation;
        private readonly HitBoxLink _dataLink = new();
        private CubePuzzleDataReader _puzzleData;
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
            _puzzleData = puzzleData;
            var instantiator = new Instantiator<Flower>(_flowerPrefab);
            _cubeMap = new CubeMap<Flower>(puzzleData.Width, instantiator);
            _flwerPresentation = new FlowerPresentation(_cubeMap);

            foreach (var index in _cubeMap.GetIndex())
            {
                var flower = _cubeMap.GetElements(index);
                _dataLink.Link(flower, index.Concat(new byte[] { 0 }).ToArray<byte>());
                puzzleData.GetPositionAndRotation(index, out var position, out var rotation);
                flower.transform.SetParent(puzzleData.BaseTransform);
                flower.transform.SetLocalPositionAndRotation(position, rotation);
                _flwerPresentation.InstreamData(index.Concat(new byte[] { puzzleData.GetElement(index) }).ToArray<byte>());
                flower.gameObject.SetActive(puzzleData.ReadWindow == (Face)index[2] && flower.Color != Color.clear && (Face)index[2] != Face.bottom);
            }
            puzzleData.OnRotatedStage += OnRotated;
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

        private void OnRotated(Face face)
        {
            if(face is Face.bottom)
            {
                return;
            }
            foreach (var index in _cubeMap.GetIndex())
            {
                var flower = _cubeMap.GetElements(index);
                if ((byte)face == index[2])
                {
                    index[2] = (byte)Face.top;
                    _puzzleData.GetPositionAndRotation(index, out var position, out var rotation);
                    flower.transform.position = _puzzleData.BaseTransform.position + position;
                    flower.transform.rotation = Quaternion.identity;
                    flower.gameObject.SetActive(flower.Color != Color.clear);
                }
                else
                {
                    flower.gameObject.SetActive(false);
                }
            }
        }

    }
}

