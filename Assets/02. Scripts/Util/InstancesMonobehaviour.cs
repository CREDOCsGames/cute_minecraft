using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Util
{
    public class InstancesMonobehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static readonly List<T> _instances = new();
        public static List<T> Instances => _instances.ToList();

        protected virtual void OnEnable()
        {
            _instances.Add(this as T);
        }

        protected virtual void OnDisable()
        {
            _instances.Remove(this as T);
        }

        protected virtual void OnDestroy()
        {
            _instances.Remove(this as T);
        }
    }
}