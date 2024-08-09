using UnityEngine;

namespace PlatformGame
{
    public class UniqueScriptableObject<T> : ScriptableObject where T : ScriptableObject
    {
        static T mInstance;
        public static T Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = Resources.Load<T>(typeof(T).Name);
                    Debug.Assert(mInstance != null, $"Not found {typeof(T)}");
                }
                return mInstance;
            }
        }
    }
}