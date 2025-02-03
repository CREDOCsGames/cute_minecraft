using System.Collections.Generic;
using UnityEngine;

namespace Puzzle
{
    public class FlowerPuzzleBossCore : MonoBehaviour, ICore, IPuzzleCore
    {
        public DataReader DataReader { get; private set; } = new FlowerReader();
        private CubeMap<byte> _puzzle;
        private IMediatorCore _mediator;
        private readonly CoreFunction _crossAttack = FlowerCoreFuntions.AttackCrossLink;
        private readonly CoreFunction _dotAttack = FlowerCoreFuntions.AttackDot;
        private readonly CoreFunction _createFlower = FlowerCoreFuntions.CreateFlower;
        private readonly CoreFunction _clearCheck = FlowerCoreFuntions.CheckFlowerBossStageClear;
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
        public void Init(CubePuzzleReaderForCore reader)
         {
            _puzzle = reader.Map;
            gameObject.SetActive(true);
            _mediator.InstreamDataCore<MonsterReader>(MonsterReader.BOSS_SPAWN);
        }
        public void SetMediator(IMediatorCore mediator)
        {
            _mediator = mediator;
        }

    }
}
