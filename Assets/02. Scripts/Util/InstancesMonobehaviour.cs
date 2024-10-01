using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Util
{
    public class InstancesMonobehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        static readonly List<T> mInstances = new();
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