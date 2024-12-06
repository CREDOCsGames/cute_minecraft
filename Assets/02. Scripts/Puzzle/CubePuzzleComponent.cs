using A;
using Movement;
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Util;

namespace Puzzle
{
    public class CubePuzzleComponent : MonoBehaviour, IInstance
    {
        public MediatorCenter.TunnelFlag Flag;
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

        public event Action<byte[]> InstreamEvent;

        void Awake()
        {
            if (!Puzzles.Where(x => x as IInstance != null).Any())
            {
                return;
            }

            _mediator = new MediatorCenter();
            _mediator.AddCore(CreateMap(), Flag);

            foreach (var puzzle in Puzzles)
            {
                if (puzzle is IInstance && puzzle is ScriptableObject obj)
                {
                    var instance = GameObject.Instantiate(obj) as IInstance;
                    _mediator.AddInstance(instance, Flag);
                }
                else
                {
                    Debug.Assert(false, $"{puzzle.name} is not IPuzzleInstance. type {puzzle.GetType()}");
                }
            }

            Invoke(nameof(A), 3);
            _mediator.AddListenerSystemMessage(this);
        }

        private void A()
        {
            _mediator.Instances.ForEach(x => (x as FlowerPuzzleInstance).Init(transform));
            _mediator.Cores.ForEach(x => (x as ICore).Init());
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

        private void Rotate(string path)
        {
            var action = Resources.Load<MovementAction>(path);
            if (action != null)
            {
                GetComponent<MovementComponent>().PlayMovement(action);
            }
        }

        public void InstreamData(byte[] data)
        {
            if (!SystemMessage.CheckSystemMessage(data))
            {
                return;
            }

            GameObject.FindAnyObjectByType<PCController>().DoJump();
            if (SystemMessage.CLEAR_RIGHT.Equals(data))
            {
                Rotate("MovementAction/RotateLeft");
            }
            else if (SystemMessage.CLEAR_LEFT.Equals(data))
            {
                Rotate("MovementAction/RotateLeft");
            }
            else if (SystemMessage.CLEAR_FRONT.Equals(data))
            {
                Rotate("MovementAction/RotateLeft");
            }
            else if (SystemMessage.CLEAR_BACK.Equals(data))
            {
                Rotate("MovementAction/RotateBackward");
            }
            else if (SystemMessage.CLEAR_TOP.Equals(data))
            {
                Rotate("MovementAction/RotateBackward");
            }
            else if (SystemMessage.CLEAR_BOTTOM.Equals(data))
            {
                Rotate("MovementAction/RotateLeft");
            }
        }
    }

}
