using System;
using System.Collections;
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

        public static void InvokeDelayAction(Action action, float delay)
        {
            Instance.StartCoroutine(DelayAction(action, delay));
        }

        static IEnumerator DelayAction(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke();
        }

    }
}
