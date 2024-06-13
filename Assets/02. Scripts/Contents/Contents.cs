using PlatformGame.Contents.Loader;
using UnityEngine;

namespace PlatformGame.Contents
{
    public enum LoaderType
    {
        StageLoader,
        CubeLoader,
        LevelLoader
    }

    public class Contents
    {
        public static Contents Instance { get; private set; }

        public WorkState State => mLoader.State;
        ILevelLoader mLoader;

        public Contents(LoaderType type)
        {
            Debug.Assert(Instance == null, $"Contents already exists.");
            Instance = this;
            SetLoaderType(type);
        }

        public void LoadNextLevel()
        {
            mLoader.LoadNext();
        }

        [VisibleEnum(typeof(LoaderType))]
        public void SetLoaderType(LoaderType type)
        {
            switch (type)
            {
                case LoaderType.StageLoader:
                    mLoader = new StageLoader();
                    break;
                case LoaderType.CubeLoader:
                    mLoader = Object.FindObjectOfType<CubeLoader>();
                    Debug.Assert(mLoader != null);
                    break;
                case LoaderType.LevelLoader:
                    mLoader = new LevelLoader();
                    break;
                default:
                    Debug.Assert(false, $"Undefined value : {type}");
                    break;
            }
        }
    }
}