using Battle;
using System.Linq;
using UnityEngine;
using Util;

namespace Puzzle
{
    [CreateAssetMenu(menuName = "Puzzle/FlowerPuzzle")]
    public class FlowerPuzzleInstance : ScriptableObject, IInstance, IPuzzleInstance, IReleasable
    {
        public DataReader DataReader { get; private set; } = FlowerReader.Instance;
        private CubeMap<Flower> _cubeMap;
        private CubePuzzleReader _puzzle;
        private IMediatorInstance _mediator;
        private Color _EMPTY => _flowerColors[0];
        [SerializeField] private ColorList _flowerColors = ColorList.DEFAULT;
        [SerializeField] private Flower _flowerPrefab = Flower.DEFAULT;

        public void InitInstance(CubePuzzleReader puzzle)
        {
            SetPuzzleData(puzzle);
            InstantiateCubeMap();
            Enermerator.InvokeFor(_cubeMap.GetIndexArray(), InitFlower);
            Enermerator.InvokeFor(_cubeMap.GetIndexArray(), DisableFlower);
            Enermerator.InvokeFor(_cubeMap.GetIndexArray(puzzle.PlayingFace), EnableFlower);
        }
        public void DoRelease()
        {
            Enermerator.InvokeFor(_cubeMap.Elements, Destroy);
            _cubeMap = null;
            _puzzle.OnRotated -= OnRotated;
        }
        private void SetPuzzleData(CubePuzzleReader puzzle)
        {
            _puzzle = puzzle;
            _puzzle.OnRotated += OnRotated;
        }
        private void InstantiateCubeMap()
        {
            var instantiator = new Instantiator<Flower>(_flowerPrefab);
            _cubeMap = new CubeMap<Flower>(_puzzle.Width, instantiator);
        }
        public void InitFlower(byte[] index)
        {
            if (_cubeMap == null)
            {
                Debug.Log(DM_ERROR.REFERENCES_NULL);
            }
            if (_cubeMap.IsOutOfRange(index))
            {
                Debug.Log($"{DM_ERROR.OUT_OF_RANGE} : {index}");
                return;
            }

            var flower = _cubeMap.GetElements(index);
            flower.transform.SetParent(_puzzle.BaseTransform);
            flower.Index = index;
            flower.OnHit -= OnHit;
            flower.OnHit += OnHit;

            var color = _puzzle.GetElement(index);
            flower.Color = _flowerColors[color];
        }
        private void EnableFlower(byte[] index)
        {
            var flower = _cubeMap.GetElements(index);
            flower.gameObject.SetActive(flower.Color != _EMPTY);
            _puzzle.GetPositionAndRotation(index, out var position, out var rotation);
            flower.transform.SetPositionAndRotation(position, rotation);
        }
        private void DisableFlower(byte[] index)
        {
            var flower = _cubeMap.GetElements(index);
            flower.gameObject.SetActive(false);
        }
        public void SetMediator(IMediatorInstance mediator)
        {
            _mediator = mediator;
        }
        public void InstreamData(byte[] data)
        {
            var index = FlowerReader.GetFlowerIndex(data);
            var flower = _cubeMap.GetElements(index);
            var flowerColor = FlowerReader.GetFlowerColor(data);
            flower.Color = _flowerColors[flowerColor];
        }
        private void OnHit(HitBoxCollision coll)
        {
            if (_mediator == null ||
                !coll.Victim.TryGetComponent<Flower>(out var flower))
            {
                return;
            }
            var message = FlowerReader.CreateHitMessage(flower.Index);
            if (coll.Attacker.TryGetComponent<PuzzleAttackBoxComponent>(out var box))
            {
                message = FlowerReader.AdditiveMessage(message, box.Type);
            }
            _mediator.InstreamDataInstance<FlowerReader>(message);
        }
        private void OnRotated(Face preFace, Face playFace)
        {
            if (preFace is not FlowerReader.SHELTER)
            {
                Enermerator.InvokeFor(_cubeMap.GetIndexArray(preFace), DisableFlower);
            }

            if (playFace is not FlowerReader.SHELTER)
            {
                Enermerator.InvokeFor(_cubeMap.GetIndexArray(playFace), EnableFlower);
            }
        }

    }
}

