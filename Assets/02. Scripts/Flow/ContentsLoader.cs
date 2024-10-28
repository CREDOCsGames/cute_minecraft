using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Flow
{
    public enum LoaderType
    {
        StageLoader,
        LevelLoader,
        ReLoader
    }

    public static class ContentsLoader
    {
        static WorkState State => mLoader.State;

        static ILevelLoader mLoader = new LevelLoader();

        static readonly List<ILevelLoader> mLoaders = new()
        {
            new StageLoader(),
            new LevelLoader(),
            new ReLoader()
        };

        public static event Action OnStartLoad;
        public static event Action OnLoaded;

        public static void LoadContents()
        {
            if (State is not WorkState.Ready)
            {
                Debug.LogWarning($"The loader is not ready : {State}");
                return;
            }

            mLoader.LoadNext();
            OnStartLoad?.Invoke();
            CoroutineRunner.Instance.StartCoroutine(CheckLoad());
        }

        public static void SetLoaderType(LoaderType type)
        {
            Debug.Assert(Enum.IsDefined(typeof(LoaderType), type), $"Out of range : {(int)type}");
            mLoader = mLoaders[(int)type];
            Debug.Assert(mLoader != null);
        }

        static IEnumerator CheckLoad()
        {
            var mWait = new WaitForSeconds(0.5f);
            while (State != WorkState.Ready)
            {
                yield return mWait;
            }

            OnLoaded?.Invoke();
        }
    }
}