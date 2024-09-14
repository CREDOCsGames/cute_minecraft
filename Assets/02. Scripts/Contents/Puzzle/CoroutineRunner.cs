using UnityEngine;
namespace PlatformGame.Util
{
    public class CoroutineRunner : MonoBehaviour
    {
        static MonoBehaviour mInstance;
        public static MonoBehaviour Instance
        {
            get
            {
                if (mInstance == null)
                {
                    var obj = new GameObject();
                    GameObject.DontDestroyOnLoad(obj);
                    mInstance = obj.AddComponent<CoroutineRunner>();
                }
                return mInstance;
            }
        }

    }
}
