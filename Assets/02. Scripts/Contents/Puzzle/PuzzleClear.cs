using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame.Contents.Puzzle
{
    [RequireComponent(typeof(LoadManager))]
    public class PuzzleClear : Singleton<PuzzleClear>
    {
        [SerializeField] UnityEvent mClearEvent;
        LoadManager mLoadManager;

        protected override void Awake()
        {
            base.Awake();
            mLoadManager = GetComponent<LoadManager>();
        }

        public void InvokeClearEvent()
        {
            StageManager.Instance.ClearCurrentStage();
            mClearEvent.Invoke();
            // mLoadManager.Load();
        }
    }

}
