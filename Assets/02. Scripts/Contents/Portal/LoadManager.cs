using UnityEngine;

namespace PlatformGame.Contents
{
    public class LoadManager : MonoBehaviour
    {
        public LoaderType LoaderType;
        public float LoadDelay;

        public void Load()
        {
            Contents.Instance.SetLoaderType(LoaderType);
            GameManager.Instance.LoadGame(LoadDelay);
        }
    }
}