using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame.Character
{
    public class CubeMapObject : MonoBehaviour
    {
        public static List<CubeMapObject> Objects { get; private set; } = new();

        public UnityEvent ForwardChange;
        public UnityEvent BackwardChange;
        public UnityEvent RightChange;
        public UnityEvent LeftChange;
        public UnityEvent UpChange;
        public UnityEvent DownChange;

        public void OnChanged(CubeMapState state)
        {
            var change = new UnityEvent();
            switch (state)
            {
                case CubeMapState.Forward:
                    change = ForwardChange;
                    break;
                case CubeMapState.Backward:
                    change = BackwardChange;
                    break;
                case CubeMapState.Left:
                    change = LeftChange;
                    break;
                case CubeMapState.Right:
                    change = RightChange;
                    break;
                case CubeMapState.Up:
                    change = UpChange;
                    break;
                case CubeMapState.Down:
                    change = DownChange;
                    break;
                default:
                    Debug.Assert(false, $"Undefined value : {nameof(CubeMapState)}, {state}.");
                    break;
            }

            change.Invoke();
        }

        void Awake()
        {
            Objects.Add(this);
        }

        void OnDestroy()
        {
            Objects.Remove(this);
        }
    }
}