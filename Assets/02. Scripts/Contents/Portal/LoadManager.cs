using PlatformGame.Contents.Loader;
using System.Collections;
using UnityEngine;

namespace PlatformGame.Contents
{
    public class LoadManager : MonoBehaviour
    {
        GameManager mGameManager;
        ContentsLoader mContentsLoader;
        public LoaderType LoaderType;
        [Range(0, 1000)] public float LoadDelay = 0f;

        public void Load()
        {
            Invoke(nameof(StartLoad), LoadDelay);
        }

        void StartLoad()
        {
            mContentsLoader.SetLoaderType(LoaderType);
            mContentsLoader.LoadContents();
        }

        void Start()
        {
            mGameManager = GameManager.Instance;
            mContentsLoader = ContentsLoader.Instance;
        }

    }
}