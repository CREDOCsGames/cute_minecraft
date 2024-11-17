using System;
using System.Collections;
using UnityEngine;

namespace Util
{
    public class CoroutineRunner : MonoBehaviour
    {
        private static MonoBehaviour _instance;

        public static MonoBehaviour Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                var obj = new GameObject();
                GameObject.DontDestroyOnLoad(obj);
                _instance = obj.AddComponent<CoroutineRunner>();

                return _instance;
            }
        }

        public static void InvokeDelayAction(Action action, float delay)
        {
            Instance.StartCoroutine(DelayAction(action, delay));
        }

        private static IEnumerator DelayAction(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke();
        }
    }
}