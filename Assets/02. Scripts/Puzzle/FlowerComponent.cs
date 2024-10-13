using Flow;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Util;

namespace Puzzle
{
    public class FlowerComponent : PuzzlePieceComponent, IColorPiece
    {
        static readonly List<FlowerComponent> mInstances = new();
        public static List<FlowerComponent> Instances => mInstances;
        [SerializeField] Color mColor;

        [Header("[Refer to the part you want the color to change]")] [SerializeField]
        List<MeshRenderer> mRenderers;

        public List<MeshRenderer> Renderers => mRenderers;
        [Header("[Options]")] [SerializeField] UnityEvent mChangeColorEvent;

        readonly Timer mFloweringTimer = new();
        [SerializeField] GameObject Seed;

        public Color Color
        {
            get => mColor;
            set
            {
                mColor = value;
                Colorize.Instance.Invoke(Renderers, Color);
                mChangeColorEvent.Invoke();
                mTimer.Stop();
                mTimer.Start();
            }
        }

        public static bool CompareColor(Color a, Color b)
        {
            return Mathf.Abs(a.r - b.r) < 0.01 &&
                   Mathf.Abs(a.g - b.g) < 0.01 &&
                   Mathf.Abs(a.b - b.b) < 0.01;
        }

        static void CheckClear()
        {
            bool bClear;
            if (!mInstances.Any())
            {
                bClear = true;
            }
            else
            {
                var color = mInstances.First().Color;
                bClear = mInstances.All(x => CompareColor(x.Color, color));
            }

            if (!bClear)
            {
                return;
            }

            mInstances.ForEach(x => x.IsClear = true);
            mInstances.ToList().ForEach(x => x.enabled = false);
            GameManager.Lantern.StoneCount++;
        }

        void OnEnable()
        {
            mInstances.Add(this);
            if (mInstances.First() != this)
            {
                return;
            }
        }

        void Awake()
        {
            Color = mColor;
            mFloweringTimer.SetTimeout(2f);
            mFloweringTimer.OnTimeoutEvent += (t) => Seed?.SetActive(true);
        }

        static readonly Timer mTimer = new(1f, (t) => CheckClear());

        void Update()
        {
            mFloweringTimer.Tick();
            if (Seed != null && !mFloweringTimer.IsStart && !Seed.activeSelf)
            {
                mFloweringTimer.Start();
            }

            if (mInstances.First() != this)
            {
                return;
            }

            mTimer.Tick();
        }

        void OnDisable()
        {
            mInstances.Remove(this);
        }
    }
}