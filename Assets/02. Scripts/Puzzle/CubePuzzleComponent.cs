using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Util;

namespace Puzzle
{
    public class CubePuzzleComponent : MonoBehaviour
    {
        public ScriptableObject[] Puzzles;
        public Scriptable_MatrixByte[] MapData
        {
            get
            {
                Scriptable_MatrixByte[] mapData = new Scriptable_MatrixByte[6];
                var path = Path.Combine("Assets/10. Editor/Cookie", this.GetInstanceID().GetHashCode().ToString());
                for (int i = 0; i < 6; i++)
                {
                    if (!File.Exists(path + $"{i}.asset"))
                    {
                        Scriptable_MatrixByte obj = ScriptableObject.CreateInstance<Scriptable_MatrixByte>();
                        AssetDatabase.CreateAsset(obj, path + $"{i}.asset");
                        AssetDatabase.SaveAssets();
                    }
                    mapData[i] = AssetDatabase.LoadAssetAtPath<Scriptable_MatrixByte>(path + $"{i}.asset");
                }
                return mapData;
            }
        }
        private MediatorCenter _mediator;

        void Awake()
        {
            if (!Puzzles.Where(x => x as IInstance != null).Any())
            {
                return;
            }

            _mediator = new MediatorCenter();
            _mediator.AddCore(CreateMap(), MediatorCenter.TunnelFlag.Flower);

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

            Invoke(nameof(A), 3);
        }

        private void A()
        {
            _mediator.Cores.ForEach(x => (x as PuzzleCore).Init());

        }

        private void OnDisable()
        {
            _mediator = null;
        }

        public CubeMap<byte> CreateMap()
        {
            var cubeMap = new CubeMap<byte>((byte)MapData[0].Matrix.ColumnsCount, (byte)0);

            for (byte face = 0; face < 6; face++)
            {
                for (byte x = 0; x < MapData[0].Matrix.ColumnsCount; x++)
                {
                    for (byte y = 0; y < MapData[0].Matrix.ColumnsCount; y++)
                    {
                        MapData[face].Matrix.TryGetElement(x, y, out var value);
                        cubeMap.SetElements(x, y, face, value);
                    }
                }
            }

            return cubeMap;
        }
    }

}
