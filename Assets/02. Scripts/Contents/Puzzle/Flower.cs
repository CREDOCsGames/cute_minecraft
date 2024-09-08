using PlatformGame.Util;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame.Contents.Puzzle
{
    public class Flower : MonoBehaviour, IColorPiece
    {
        static List<Flower> mInstances = new();
        [SerializeField] Color mColor;

        [SerializeField] List<MeshRenderer> mRenderers;
        public List<MeshRenderer> Renderers => mRenderers;
        [SerializeField] UnityEvent mChangeColorEvent;

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

        static bool CompareColor(Color a, Color b)
        {
            bool _is = false;
            if(Mathf.Abs(a.a -  b.a) < 0.01 &&
                Mathf.Abs(a.r - b.r) < 0.01 &&
                Mathf.Abs(a.g - b.g) < 0.01 &&
                Mathf.Abs(a.b - b.b) < 0.01 )
            {
                _is = true;
            }
            return _is; 
        }

        static void CheckClear()
        {
            var color = mInstances.First().Color;
            var bClear = mInstances.All(x => CompareColor(x.Color,color));

            if (bClear)
            {
                PuzzleClear.Instance.InvokeClearEvent();
            }
        }

        void Awake()
        {
            mInstances.Add(this);
            var material = new Material(mRenderers.First().material);
            foreach (var renderer in mRenderers)
            {
                renderer.material = material;
            }


            if (mInstances.First() != this)
            {
                return;
            }

            mTimer.OnTimeoutEvent += (t) => CheckClear();
            mTimer.SetTimeout(1f);
        }

        static Timer mTimer = new();
        void Update()
        {
            if (mInstances.First() != this)
            {
                return;
            }

            mTimer.Tick();
        }

        void OnDestroy()
        {
            mInstances.Remove(this);
        }
    }
}
