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
                CheckClear();
                Colorize.Instance.Invoke(Renderers, Color);
                mChangeColorEvent.Invoke();
            }
        }

        static void CheckClear()
        {
            var color = mInstances.First().Color;
            var bClear = mInstances.All(x => x.Color.Equals(color));

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
        }

        void OnDestroy()
        {
            mInstances.Remove(this);
        }
    }
}
