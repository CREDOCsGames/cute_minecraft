using UnityEngine;

namespace PlatformGame
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
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
        protected virtual void Awake()
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

        protected virtual void OnDestroy()
        {
            if (mInstance == this)
            {
                mInstance = null;
            }
        }

    }

}
