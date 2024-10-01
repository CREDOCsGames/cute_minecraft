using UnityEngine;

namespace Util
{
    public class DontDestroyObjectComponent : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}