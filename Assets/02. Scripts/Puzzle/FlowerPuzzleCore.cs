using UnityEngine;

namespace Puzzle
{
    [CreateAssetMenu(menuName = "Custom/FlowerPuzzleCore")]
    public class FlowerPuzzleCore : ScriptableObject, ICore, IPuzzleCore, IReleasable
    {
        public DataReader DataReader { get; private set; } = FlowerReader.Instance;
        private CubeMap<byte> _puzzle;
        private IMediatorCore _mediator;
        private CubePuzzleReaderForCore _reader;
        private readonly CoreSetting _coreSetting = new();
        private readonly CoreFunction _crossAttack = FlowerCoreFuntions.AttackCross;
        private readonly CoreFunction _dotAttack = FlowerCoreFuntions.AttackDot;
        private readonly CoreFunction _createFlower = FlowerCoreFuntions.CreateFlower;
        private readonly CoreFunction _checkClear = FlowerCoreFuntions.CheckClearFlowerNormalStage;

        public void InstreamData(byte[] data)
        {
            if (!_coreSetting.IsStart)
            {
                return;
            }

            var eventData = FlowerReader.GetEventData(data);
            var puzzleEvent = eventData switch
            {
                FlowerReader.EVENT_DOT => _dotAttack,
                FlowerReader.EVENT_CROSS => _crossAttack,
                FlowerReader.EVENT_CREATE => _createFlower,
                _ => FlowerCoreFuntions.PrintDebugMessage
            };

            puzzleEvent(data, _puzzle, out var puzzleMessages);
            foreach (var message in puzzleMessages)
            {
                _mediator.InstreamDataCore<FlowerReader>(message);
            }

            _checkClear(data, _puzzle, out var clearMessage);
            foreach (var message in clearMessage)
            {
                _mediator.InstreamDataCore<SystemReader>(message);
            }
        }
        public void Init(CubePuzzleReaderForCore reader)
        {
            _reader = reader;
            _puzzle = reader.Map;
            _reader.PuzzleEvent.OnStartLevel += _coreSetting.StartCore;
            _reader.PuzzleEvent.OnClearLevel += _coreSetting.StopCore;
        }
        public void DoRelease()
        {
            _reader.PuzzleEvent.OnStartLevel -= _coreSetting.StartCore;
            _reader.PuzzleEvent.OnClearLevel -= _coreSetting.StopCore;
            _reader = null;
            _puzzle = null;
        }
        public void SetMediator(IMediatorCore mediator)
        {
            _mediator = mediator;
        }
    }
}
