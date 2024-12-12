using Battle;
using Movement;
using System;
using System.IO;
using System.Linq;
using UnityEngine;
using Util;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Puzzle
{
    [ExecuteAlways]
    public class CubePuzzleComponent : MonoBehaviour, IInstance
    {
        [HideInInspector][SerializeField] private string _uniqueID;
        [SerializeField][Range(1, 255)] private byte _width;
        public MediatorCenter.TunnelFlag Flag;
        public ScriptableObject[] Puzzles;
        public Scriptable_MatrixByte[] MapData
        {
            get
            {
                Scriptable_MatrixByte[] mapData = new Scriptable_MatrixByte[6];
                var path = Path.Combine("Assets/10. Editor/Cookie", GetUniqueID());
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

        private string GetUniqueID()
        {
            if (string.IsNullOrEmpty(_uniqueID))
            {
                _uniqueID = GUID.Generate().ToString();
#if UNITY_EDITOR
                EditorUtility.SetDirty(this);
#endif
            }
            return _uniqueID;
        }

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
                    (instance as PuzzleInstance).Init(new PuzzleCubeData());
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

        string p;
        private void Rotate(string path)
        {
            p = path;   
            Invoke(nameof(Go), 1f);
        }

        private void Go()
        {
            var action = Resources.Load<MovementAction>(p);
            if (action != null)
            {
                GetComponent<MovementComponent>().PlayMovement(action);
            }
            GameObject.FindAnyObjectByType<PCController>()?.DoJump(500);
        }

        public void InstreamData(byte[] data)
        {
            if (!SystemMessage.CheckSystemMessage(data))
            {
                return;
            }


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

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(transform.position, Vector3.one * _width);
        }
    }

}
