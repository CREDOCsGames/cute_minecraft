using System.Linq;
using UnityEngine;
using Util;

namespace Puzzle
{
    public class CubePuzzleComponent : MonoBehaviour
    {
        public ScriptableObject[] Puzzles;
        public Scriptable_MatrixByte MapData;
        private MediatorCenter _mediator;
        void OnEnable()
        {
            if (!Puzzles.Where(x => x as IInstance != null).Any())
            {
                return;
            }

            _mediator = new MediatorCenter();
            foreach (var puzzle in Puzzles)
            {
                if (puzzle is IInstance && puzzle is ScriptableObject obj)
                {
                    var instance = GameObject.Instantiate(obj) as IInstance;
                    _mediator.AddInstance(instance, MediatorCenter.TunnelFlag.Flower);
                }
                else
                {
                    Debug.Assert(false, $"{puzzle.name} is not IPuzzleInstance. type {puzzle.GetType()}");
                }
            }
        }

        private void OnDisable()
        {
            _mediator = null;
        }

        public CubeMap<byte> CreateMap(MatrixBool[] map)
        {
            var cubeMap = new CubeMap<byte>((byte)map[0].Column, (byte)0);

            for (byte face = 0; face < 6; face++)
            {
                for (byte x = 0; x < map[0].Column; x++)
                {
                    for (byte y = 0; y < map[0].Column; y++)
                    {
                        cubeMap.SetElements(x, y, face, map[face].Matrix[y].List[x] == false ? (byte)0 : (byte)1);
                    }
                }
            }

            return cubeMap;
        }
    }

}
