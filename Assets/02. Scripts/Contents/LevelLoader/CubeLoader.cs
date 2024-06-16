using PlatformGame.Input;
using UnityEngine;
using static PlatformGame.Input.ActionKey;

namespace PlatformGame.Contents.Loader
{
    public class CubeLoader : MonoBehaviour, ILevelLoader
    {
        public WorkState State { get; private set; }
        [SerializeField] Character.Controller.ActionController mCubeController;

        void Awake()
        {
            Debug.Assert(mCubeController != null);
            mCubeController.SetActive(false);
        }

        void Update()
        {
            if (State != WorkState.Action)
            {
                return;
            }

            var map = ActionKey.GetKeyDownMap();
            if (!map[KEY_GUARD])
            {
                return;
            }

            State = WorkState.Ready;
            mCubeController.SetActive(false);
        }

        public void LoadNext()
        {
            State = WorkState.Action;
            mCubeController.SetActive(true);
        }
    }
}