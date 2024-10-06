using UnityEngine;
using UnityEngine.Events;

namespace Flow
{
    public class LoadManagerComponent : MonoBehaviour
    {
        public LoaderType LoaderType;
        [Range(0, 1000)] public float LoadDelay = 0f;
        [SerializeField] UnityEvent LoadEvent;

        public void Load()
        {
            Invoke(nameof(StartLoad), LoadDelay);
        }

        void StartLoad()
        {
            LoadEvent.Invoke();
            ContentsLoader.SetLoaderType(LoaderType);
            ContentsLoader.LoadContents();
        }
    }
}