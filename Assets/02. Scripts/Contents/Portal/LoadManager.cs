using UnityEngine;

namespace PlatformGame.Contents
{
    public class LoadManager : MonoBehaviour
    {
        public LoaderType LoaderType;
        [Range(0, 1000)] public float LoadDelay = 0f;

        public void Load()
        {
            Invoke(nameof(StartLoad), LoadDelay);
        }

        void StartLoad()
        {
            ContentsLoader.SetLoaderType(LoaderType);
            ContentsLoader.LoadContents();
        }
    }
}