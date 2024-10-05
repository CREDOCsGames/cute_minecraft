using PlatformGame.Util;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame.Contents
{
    [Flags]
    public enum BetteryType
    {
        None = 0,
        MinusPole = 1 << 0
    }

    public class Bettery : InstancesMonobehaviour<Bettery>
    {
        [SerializeField, Range(1, 100)] float mCapacity;
        public float Capacity => mCapacity;
        [SerializeField, Range(0, 100)] float mAmount;
        public float Amount
        {
            get => mAmount;
            set
            {
                var newValue = Mathf.Clamp(value, 0, mCapacity);
                if (mAmount < newValue)
                {
                    OnCharge.Invoke(newValue);
                }
                else if (newValue < mAmount)
                {
                    OnUse.Invoke(newValue, mCapacity);
                }
                mAmount = Mathf.Clamp(value, 0, mCapacity);
                if (IsEmpty)
                {
                    OnDischarge.Invoke();
                }
            }
        }

        [SerializeField] BetteryType mType;
        public BetteryType Type => mType;
        public bool IsPlusPole => !mType.HasFlag(BetteryType.MinusPole);
        public bool IsMinusPole => mType.HasFlag(BetteryType.MinusPole);
        public bool IsEmpty => Amount == 0;
        public UnityEvent OnDischarge;
        public UnityEvent<float> OnCharge;
        public UnityEvent<float, float> OnUse;
        public void FullChargeBettery()
        {
            Amount = mCapacity;
        }
    }
}
