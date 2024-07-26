using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame.Contents.Compartment
{
    public class Compartments : MonoBehaviour
    {
        public List<Transform> YellowColors;
        public List<Transform> GreenColors;
        public List<Transform> BlueColors;
        public List<Transform> OrangeColors;
        public List<Transform> RedColors;
        public List<Transform> WhiteColors;
        List<List<Transform>> Colors;
        public Color SymbolColor;
        [SerializeField] Color mPaintedColor;
        public Color PaintedColor
        {
            get => mPaintedColor;
            private set
            {
                mPaintedColor = value;
                if (mPaintedColor == Color.None)
                {
                    return;
                }

                if (mPaintedColor == SymbolColor)
                {
                    OnClear();
                }
                else
                {
                    OnFail();
                }
                mTimer.Start();
            }
        }
        public UnityEvent OnClearEvent;
        public UnityEvent OnFailEvent;
        public UnityEvent DeactivationEvent;
        public float ActivationDuration;
        public bool IsActivation => mTimer.IsStart;
        readonly Timer mTimer = new();

        public void ColorizeCompartments(Transform transform)
        {
            if (IsActivation)
            {
                return;
            }
            PaintedColor = FindColor(transform);
        }

        public void RemoveTimeout()
        {
            mTimer.RemoveTimeout();
        }

        Color FindColor(Transform transform)
        {
            var index = Colors.FindIndex(x => x.Contains(transform));
            Debug.Assert(index >= 0, $"Not in the list : {transform.name}");
            return (Color)index + 1;
        }

        void OnFail()
        {
            OnFailEvent.Invoke();
        }

        void OnClear()
        {
            OnClearEvent.Invoke();
        }

        public void Deactivation()
        {
            mTimer.Stop();
            DeactivationEvent.Invoke();
        }

        void Awake()
        {
            mTimer.OnTimeoutEvent += (t) => Deactivation();
            mTimer.SetTimeout(ActivationDuration);

            Colors = new()
            {
                YellowColors,
                GreenColors,
                BlueColors,
                OrangeColors,
                RedColors,
                WhiteColors
            };

            if (mPaintedColor != Color.None)
            {
                PaintedColor = mPaintedColor;
            }
        }

        void Update()
        {
            if (!IsActivation)
            {
                return;
            }

            mTimer.Tick();
        }

    }
}

