using System;
using UnityEngine;

namespace Util
{
    [Flags]
    public enum CountEventFlag
    {
        None = 0,
        Clamp = 1 << 0,
        Unsigned = 1 << 1,
        OneTime = 1 << 2
    }

    public class CountEvent
    {
        public int NumberOfGoals = 1;
        public event Action CountReachedEvent;
        public event Action CountAddEvent;
        private int _maxValue = int.MaxValue;
        private int _minValue = int.MinValue;
        private int _count;
        private bool _oneTime;
        private CountEventFlag _options;

        public int Count
        {
            get => _count;

            set
            {
                _count = value;
                if (_options.HasFlag(CountEventFlag.Clamp))
                {
                    _count = Mathf.Clamp(_count, _minValue, _maxValue);
                }

                if (_options.HasFlag(CountEventFlag.Unsigned))
                {
                    _count = Mathf.Clamp(_count, 0, _count);
                }

                CountAddEvent?.Invoke();

                if (_count != NumberOfGoals)
                {
                    return;
                }

                if (_options.HasFlag(CountEventFlag.OneTime) && _oneTime)
                {
                    return;
                }

                CountReachedEvent?.Invoke();
            }
        }

        public void UseClamp(Vector2Int range)
        {
            _options |= CountEventFlag.Clamp;
            _minValue = Mathf.Min(range.x, range.y);
            _maxValue = Mathf.Max(range.x, range.y);
        }

        public void DisuseClamp()
        {
            _options &= ~CountEventFlag.Clamp;
        }

        public void UseUnsigned()
        {
            _options |= CountEventFlag.Unsigned;
        }

        public void DisuseUnsigned()
        {
            _options &= ~CountEventFlag.Unsigned;
        }

        public void UseOneTime()
        {
            _options |= CountEventFlag.OneTime;
        }

        public void DisuseOneTime()
        {
            _options &= ~CountEventFlag.OneTime;
        }
    }
}