using System.Collections.Generic;
using UnityEngine;

namespace Puzzle
{
    [CreateAssetMenu(menuName = "Custom/FlowerPuzzleCore")]
    public class FlowerPuzzleCore : ScriptableObject, ICore, IPuzzleCore
    {
        public DataReader DataReader { get; private set; } = new FlowerReader();
        private CubeMap<byte> _puzzle;
        private IMediatorCore _mediator;
        private readonly CoreFunction _crossAttack = FlowerCoreFuntions.AttackCross;
        private readonly CoreFunction _dotAttack = FlowerCoreFuntions.AttackDot;
        private readonly CoreFunction _createFlower = FlowerCoreFuntions.CreateFlower;
        private readonly CoreFunction _clearCheck = FlowerCoreFuntions.CheckFlowerNormalStageClear;

        public void InstreamData(byte[] data)
        {
            List<byte[]> puzzleMessages = new();
            switch (data[3])
            {
                case 1:
                    _crossAttack(data, _puzzle, out puzzleMessages);
                    break;
                case 2:
                    _dotAttack(data, _puzzle, out puzzleMessages);
                    break;
                case 3:
                    _createFlower(data, _puzzle, out puzzleMessages);
                    break;
                default:
                    return;
            }
            foreach (var message in puzzleMessages)
            {
                _mediator.InstreamDataCore<FlowerReader>(message);
            }

            _clearCheck(data, _puzzle, out var systemMessages);
            foreach (var message in systemMessages)
            {
                _mediator.InstreamDataCore<SystemReader>(message);
            }
        }
        public void Init(CubeMap<byte> map)
        {
            _puzzle = map;
        }
        public void SetMediator(IMediatorCore mediator)
        {
            _mediator = mediator;
        }

    }
}
