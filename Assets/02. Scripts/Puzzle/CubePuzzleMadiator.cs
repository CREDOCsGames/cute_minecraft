using System;
using System.Collections.Generic;
using Util;

namespace Puzzle
{

    public class CubePuzzleMadiator
    {
        readonly CubeMap<byte> mCubeMap;
        readonly List<IPuzzleInstance> mPuzzleInstances = new();
        readonly Dictionary<Type, Mediator> mMadiatorMap = new();
        readonly Dictionary<Type, IPuzzleCore> mCoreMap = new();

        public CubePuzzleMadiator(MatrixBool mapData)
        {
            mCubeMap = new CubeMap<byte>((byte)mapData.Column, (byte)0);
            for (byte face = 0; face < 6; face++)
            {
                for (byte y = 0; y < mCubeMap.Width; y++)
                {
                    for (byte x = 0; x < mCubeMap.Width; x++)
                    {
                        mCubeMap.SetElements(x, y, face, mapData.Matrix[y].List[x] == false ? (byte)0 : (byte)UnityEngine.Random.Range(0, 3));
                    }
                }
            }
        }

        public void AddPuzzle(IPuzzleInstance puzzle)
        {
            mPuzzleInstances.Add(puzzle);

            mMadiatorMap.TryAdd(puzzle.GetType(), new Mediator());
            var madiator = mMadiatorMap[puzzle.GetType()];

            madiator.AddInstance(puzzle);
            puzzle.Mediator = madiator;

            mCoreMap.TryAdd(puzzle.GetType(), PuzzleFactory.CreateCoreAs(puzzle, madiator, mCubeMap));
            madiator.Core = mCoreMap[puzzle.GetType()];

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
