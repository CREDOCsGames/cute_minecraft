using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame.Contents.Compartment
{
    public enum Color
    {
        None, Yellow, Green, Blue, Orange, Red, White
    }

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
            set
            {
                mPaintedColor = value;
                if(mPaintedColor == Color.None)
                {
                    return;
                }
                mbInfinitelyActive = SymbolColor == mPaintedColor;
                Activetion();
            }
        }
        UnityEngine.Color[] ColorArray =
        {
            UnityEngine.Color.magenta,
            UnityEngine.Color.yellow,
            UnityEngine.Color.green,
            UnityEngine.Color.blue,
            new UnityEngine.Color(255,165,0),
            UnityEngine.Color.red,
            UnityEngine.Color.white,
        };
        public UnityEvent ActivationEvent;
        public UnityEvent DeactivationEvent;
        public float ActivationDuration;
        float mElapsedTime = -1f;
        bool mbInfinitelyActive;
        public bool InfinitelyActive
        {
            get => mbInfinitelyActive;
            set => mbInfinitelyActive = value;

        }
        public bool IsActivation => 0 <= mElapsedTime;

        public void ColorizeCompartments(Transform transform)
        {
            if (IsActivation)
            {
                return;
            }
            PaintedColor = FindColor(transform);
        }

        public void SetEffectColor(LineRenderer renderer)
        {
            renderer.startColor = ColorArray[(int)mPaintedColor];
            renderer.endColor = ColorArray[(int)mPaintedColor];
        }

        Color FindColor(Transform transform)
        {
            var index = Colors.FindIndex(x => x.Contains(transform));
            Debug.Assert(index >= 0, $"Not in the list : {transform.name}");
            return (Color)index + 1;
        }

        public void Activetion()
        {
            mElapsedTime = 0f;
            ActivationEvent.Invoke();
        }

        public void Deactivation()
        {
            DeactivationEvent.Invoke();
        }

        void Awake()
        {
            Colors = new()
            {
                YellowColors,
                GreenColors,
                BlueColors,
                OrangeColors,
                RedColors,
                WhiteColors
            };

            if(mPaintedColor != Color.None)
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

            if (mbInfinitelyActive)
            {
                return;
            }

            if (ActivationDuration <= mElapsedTime)
            {
                mElapsedTime = -1f;
                Deactivation();
                return;
            }

            mElapsedTime += Time.deltaTime;
        }
    }
}

