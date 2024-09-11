using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PlatformGame.Util
{
    public class InstancesMonobehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        static List<T> mInstances = new();
        public static List<T> Instances => mInstances.ToList();

        protected virtual void OnEnable()
        {
            mInstances.Add(this as T);
        }

        protected virtual void OnDisable()
        {
            mInstances.Remove(this as T);
        }

        protected virtual void OnDestroy()
        {
            mInstances.Remove(this as T);
        }

    }
}
