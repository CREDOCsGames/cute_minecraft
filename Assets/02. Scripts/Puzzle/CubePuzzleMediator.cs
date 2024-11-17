using System;
using System.Collections.Generic;

namespace Puzzle
{

    public class CubePuzzleMediator
    {
        private readonly CubeMap<byte> _cubeMap;
        private readonly List<IPuzzleInstance> _puzzleInstances = new();
        private readonly Dictionary<Type, Mediator> _mediatorMap = new();
        private readonly Dictionary<Type, IPuzzleCore> _coreMap = new();

        public CubePuzzleMediator(byte width)
        {
            _cubeMap = new CubeMap<byte>(width, (byte)0);
        }

        public void AddPuzzle(IPuzzleInstance puzzle)
        {
            _puzzleInstances.Add(puzzle);

            _mediatorMap.TryAdd(puzzle.GetType(), new Mediator());
            var mediator = _mediatorMap[puzzle.GetType()];

            mediator.AddInstance(puzzle);
            puzzle.Madiator = mediator;

            _coreMap.TryAdd(puzzle.GetType(), PuzzleFactory.CreateCoreAs(puzzle, mediator, _cubeMap));
            mediator.Core = _coreMap[puzzle.GetType()];



            for (byte face = 0; face < 6; face++)
            {
                for (byte y = 0; y < _cubeMap.Width; y++)
                {
                    for (byte x = 0; x < _cubeMap.Width; x++)
                    {
                        _cubeMap.SetElements(x, y, face, puzzle.PuzzleMap.Matrix[y].List[x] == false ? (byte)0 : (byte)UnityEngine.Random.Range(0, 3));
                    }
                }
            }

        }

        public void Init()
        {
            foreach (var core in _coreMap.Values)
            {
                core.Init();
            }
        }

    }
}
