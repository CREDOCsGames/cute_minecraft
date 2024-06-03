using System;
using UnityEngine;

namespace PlatformGame.Character
{
    public enum CubeMapState
    {
        Forward,
        Backward,
        Left,
        Right,
        Up,
        Down
    }

    public class CubeMap : MonoBehaviour
    {
        static CubeMap mInstance;

        public static CubeMap Instance
        {
            get
            {
                Debug.Assert(mInstance, $"CubeMap not unique : {mInstance.name}");
                return mInstance;
            }
            private set => mInstance = value;
        }

        [SerializeField] CubeMapState mState;

        public CubeMapState State
        {
            get => mState;
            private set => mState = value;
        }

        [VisibleEnum(typeof(CubeMapState))]
        public void ChangeState(int newState)
        {
            Debug.Assert(0 <= newState &&
                         newState < Enum.GetValues(typeof(CubeMapState)).Length);
            State = (CubeMapState)newState;
            foreach (var obj in CubeMapObject.Objects)
            {
                obj.gameObject.SetActive(true);
                obj.OnChanged(State);
            }
        }

        public void OnChanging()
        {
            foreach (var obj in CubeMapObject.Objects)
            {
                obj.gameObject.SetActive(false);
            }
        }

        void Awake()
        {
            Instance = this;
        }
    }
}