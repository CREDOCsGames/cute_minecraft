using PlatformGame.Contents.Loader;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame.Contents
{
    public enum LoaderType
    {
        StageLoader,
        CubeLoader,
        LevelLoader
    }

    public class ContentsLoader : Singleton<ContentsLoader>
    {
        public WorkState State => mLoader.State;

        ILevelLoader mLoader = new LevelLoader();
        List<ILevelLoader> mLoaders = new()
        {
            new StageLoader(),
            null,
            new LevelLoader(),
        };
        [SerializeField] UnityEvent OnStartLoad;
        [SerializeField] UnityEvent OnLoaded;

        public void LoadContents()
        {
            if (!(State is WorkState.Ready))
            {
                Debug.LogWarning($"The loader is not ready : {State}");
                return;
            }
            mLoader.LoadNext();
            OnStartLoad.Invoke();
            StartCoroutine(CheckLoad());
        }

        public void SetLoaderType(LoaderType type)
        {
            switch (type)
            {
                case LoaderType.StageLoader:
                case LoaderType.LevelLoader:
                    mLoader = mLoaders[(int)type];
                    break;
                case LoaderType.CubeLoader:
                    mLoader = FindAnyObjectByType<CubeLoader>();
                    break;
                default:
                    Debug.Assert(false, $"Undefined : {type}");
                    break;
            }
            Debug.Assert(mLoader != null);
        }

        public void AddOnStartLoadEvent(UnityAction action)
        {
            OnStartLoad.AddListener(action);
        }

        public void RemoveOnStartLoadEvent(UnityAction action)
        {
            OnStartLoad.RemoveListener(action);
        }

        public void AddOnLoadedEvent(UnityAction action)
        {
            OnLoaded.AddListener(action);
        }

        public void RemoveOnLoadedEvent(UnityAction action)
        {
            OnLoaded.RemoveListener(action);
        }

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        IEnumerator CheckLoad()
        {
            WaitForSeconds mWait = new WaitForSeconds(0.5f);
            while (State != WorkState.Ready)
            {
                yield return mWait;
            }

            OnLoaded.Invoke();
        }
    }
}