using PlatformGame.Manager;
using PlatformGame.Util;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame.Contents.Puzzle
{
    public class Flower : PuzzlePiece, IColorPiece
    {
        static List<Flower> mInstances = new();
        [SerializeField] Color mColor;
        [Header("[Refer to the part you want the color to change]")]
        [SerializeField] List<MeshRenderer> mRenderers;
        public List<MeshRenderer> Renderers => mRenderers;
        [Header("[Options]")]
        [SerializeField] UnityEvent mChangeColorEvent;

        Timer mFloweringTimer = new();
        [SerializeField] GameObject Seed;

        public Color Color
        {
            get => mColor;
            set
            {
                mColor = value;
                mTimer.Stop();
                mTimer.Start();
                Colorize.Instance.Invoke(Renderers, Color);
                mChangeColorEvent.Invoke();
            }
        }

        public static bool CompareColor(Color a, Color b)
        {
            bool _is = false;
            if (Mathf.Abs(a.r - b.r) < 0.01 &&
                Mathf.Abs(a.g - b.g) < 0.01 &&
                Mathf.Abs(a.b - b.b) < 0.01)
            {
                _is = true;
            }
            return _is;
        }

        static void CheckClear()
        {
            bool bClear;
            if(!mInstances.Any())
            {
                bClear = true;
            }
            else
            {
                var color = mInstances.First().Color;
                bClear = mInstances.All(x => CompareColor(x.Color, color));
            }

            if (bClear)
            {
                GameManager.PuzzleArea.OnClear();
            }
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

        static Timer mTimer = new(1f,(t)=>CheckClear());
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
