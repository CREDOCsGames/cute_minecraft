using System;
using System.Collections.Generic;

namespace Puzzle
{

    public class CubePuzzleMadiator
    {
        readonly CubeMap<byte> mCubeMap;
        readonly List<IPuzzleInstance> mPuzzleInstances = new();
        readonly Dictionary<Type, Madiator> mMadiatorMap = new();
        readonly Dictionary<Type, IPuzzleCore> mCoreMap = new();

        public CubePuzzleMadiator(byte Width)
        {
            mCubeMap = new CubeMap<byte>(Width, (byte)0);
        }

        public void AddPuzzle(IPuzzleInstance puzzle)
        {
            mPuzzleInstances.Add(puzzle);

            mMadiatorMap.TryAdd(puzzle.GetType(), new Madiator());
            var madiator = mMadiatorMap[puzzle.GetType()];

            madiator.AddInstance(puzzle);
            puzzle.Madiator = madiator;

            mCoreMap.TryAdd(puzzle.GetType(), PuzzleFactory.CreateCoreAs(puzzle, madiator, mCubeMap));
            madiator.Core = mCoreMap[puzzle.GetType()];



            for (byte face = 0; face < 6; face++)
            {
                for (byte y = 0; y < mCubeMap.Width; y++)
                {
                    for (byte x = 0; x < mCubeMap.Width; x++)
                    {
                        mCubeMap.SetElements(x, y, face, puzzle.PuzzleMap.Matrix[y].List[x] == false ? (byte)0: (byte)UnityEngine.Random.Range(0,3));
                    }
                }
            }

        }

        public void Init()
        {
            foreach(var core in mCoreMap.Values)
            {
                core.Init();
            }
        }

    }
}
