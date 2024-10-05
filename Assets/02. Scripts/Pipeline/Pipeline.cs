using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame.Pipeline
{
    [Serializable]
    public class Pipeline<T>
    {
        [SerializeField] List<UnityEvent<T>> mPipes;
        public int Count => mPipes.Count;

        public void Invoke(T value)
        {
            for (var i = mPipes.Count - 1; i >= 0; i--)
            {
                var action = mPipes[i];
                action?.Invoke(value);
            }
        }

        public void InsertPipe(UnityAction<T> action)
        {
            var unityEvent = new UnityEvent<T>();
            unityEvent.AddListener(action);
            mPipes.Add(unityEvent);
        }

        public static Pipeline<T> CopyTo(Pipeline<T> origin)
        {
            var copy = new Pipeline<T>
            {
                mPipes = new List<UnityEvent<T>>()
            };

            foreach (var pipe in origin.mPipes)
            {
                copy.mPipes.Add(pipe);
            }

            return copy;
        }
    }
}