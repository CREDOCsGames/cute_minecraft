using Unity.VisualScripting;
using UnityEngine;
namespace PlatformGame.Util
{
    public class CoroutineRunner : MonoBehaviour
    {
        static MonoBehaviour mInstance;
        public static MonoBehaviour Instance => mInstance;

        void Awake()
        {
            mInstance = this;
        }
    }
}
