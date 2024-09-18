using System;
using UnityEngine;

namespace PlatformGame.Manager
{
    public class Area
    {
        public static Bounds zero = new ();
        Bounds mRange;
        public Bounds Range
        {
            get => mRange;
            set
            {
                Debug.Assert(value.Equals(zero) || Range.Equals(zero),$"Area overlapped : {Range}, {value}");
                mRange = value;
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