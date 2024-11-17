using UnityEngine;

namespace Util
{
    public class UniqueScriptableObject<T> : ScriptableObject where T : ScriptableObject
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                _instance = Resources.Load<T>(typeof(T).Name);
                Debug.Assert(_instance != null, $"Not found {typeof(T)}");

                return _instance;
            }
        }
    }
}