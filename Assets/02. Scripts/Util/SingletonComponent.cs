using UnityEngine;

namespace Util
{
    public sealed class SingletonComponent<T> : MonoBehaviour where T : MonoBehaviour
    {
        static T mInstance;

        public static T Instance
        {
            get
            {
                Debug.Assert(mInstance != null, $"Not found : {typeof(T)}");
                return mInstance;
            }
            private set => mInstance = value;
        }

        private void Awake()
        {
            if (mInstance == null)
            {
                mInstance = this as T;
            }
            else
            {
                Debug.LogWarning($"instance already exists. removed it : {name}");
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (mInstance == this)
            {
                mInstance = null;
            }
        }
    }
}