using System;
using UnityEngine;

namespace Flow
{
    public class Area
    {
        public static Bounds zero = new();
        private Bounds _range;

        public Bounds Range
        {
            get => _range;
            set
            {
                Debug.Assert(value.Equals(zero) || Range.Equals(zero), $"Area overlapped : {Range}, {value}");
                _range = value;
            }
        }

        public event Action<Bounds> OnEnterEvent;
        public event Action<Bounds> OnExitEvent;
        public event Action<Bounds> OnClearEvent;

        public void OnEnter()
        {
            OnEnterEvent?.Invoke(Range);
        }

        public void OnExit()
        {
            OnExitEvent?.Invoke(Range);
        }

        public void OnClear()
        {
            OnClearEvent?.Invoke(Range);
        }

    }
}