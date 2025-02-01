#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Util
{
    public static class ScriptableObejctLoader<T> where T : ScriptableObject
    {
        public static T LoadOrGenerate()
        {
            string path = typeof(T).Name;
            T _instance = Resources.Load<T>(path);
            if (!_instance)
            {
                _instance = ScriptableObject.CreateInstance<T>();
#if UNITY_EDITOR
                AssetDatabase.CreateAsset(_instance, $"Assets/Resources/{path}.asset");
                AssetDatabase.SaveAssets();
#endif
            }
            return _instance;
        }
    }
}