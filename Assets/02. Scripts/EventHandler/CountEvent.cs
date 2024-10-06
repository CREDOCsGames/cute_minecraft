using System;
using UnityEngine;

namespace PlatformGame.Event
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
        int mMaxValue = int.MaxValue;
        int mMinValue = int.MinValue;
        int mCount;
        bool mbOneTime;
        CountEventFlag mOptions;
        public int Count
        {
            get
            {
                return mCount;
            }

            set
            {
                mCount = value;
                if (mOptions.HasFlag(CountEventFlag.Clamp))
                {
                    mCount = Mathf.Clamp(mCount, mMinValue, mMaxValue);
                }

                if (mOptions.HasFlag(CountEventFlag.Unsigned))
                {
                    mCount = Mathf.Clamp(mCount, 0, mCount);
                }

                CountAddEvent?.Invoke();

                if (mCount != NumberOfGoals)
                {
                    return;
                }

                if (mOptions.HasFlag(CountEventFlag.OneTime) && mbOneTime)
                {
                    return;
                }

                CountReachedEvent?.Invoke();
            }
        }

        public void UseClamp(Vector2Int range)
        {
            mOptions |= CountEventFlag.Clamp;
            mMinValue = Mathf.Min(range.x, range.y);
            mMaxValue = Mathf.Max(range.x, range.y);
        }

        public void DisuseClamp()
        {
            mOptions &= ~CountEventFlag.Clamp;
        }

        public void UseUnsigned()
        {
            mOptions |= CountEventFlag.Unsigned;
        }

        public void DisuseUnsigned()
        {
            mOptions &= ~CountEventFlag.Unsigned;
        }

        public void UseOneTime()
        {
            mOptions |= CountEventFlag.OneTime;
        }
        public void DisuseOneTime()
        {
            mOptions &= ~CountEventFlag.OneTime;
        }

    }
}
