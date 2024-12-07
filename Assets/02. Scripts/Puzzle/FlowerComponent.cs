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
        static readonly List<FlowerComponent> _instances = new();
        public static List<FlowerComponent> Instances => _instances;
        [SerializeField] Color _color;

        [Header("[Refer to the part you want the color to change]")]
        [SerializeField]
        List<MeshRenderer> _renderers;

        public List<MeshRenderer> Renderers => _renderers;
        [Header("[Options]")][SerializeField] UnityEvent _changeColorEvent;

        readonly Timer _floweringTimer = new();
        [SerializeField] GameObject _seed;

        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                Colorize.Instance.Invoke(Renderers, Color);
                _changeColorEvent.Invoke();
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

        private static void CheckClear()
        {
            bool bClear;
            if (_instances.Count < 2)
            {
                bClear = false;
            }
            else
            {
                var color = _instances.First().Color;
                bClear = _instances.All(x => CompareColor(x.Color, color));
            }

            if (!bClear)
            {
                return;
            }

            if (GameManager.Lantern == null)
            {
                return;
            }
            GameManager.Lantern.StoneCount++;
        }

        private void OnEnable()
        {
            _instances.Add(this);
        }

        private void Awake()
        {
            Color = _color;
            _floweringTimer.SetTimeout(2f);
            _floweringTimer.OnTimeoutEvent += (t) => _seed?.SetActive(true);
        }

        private static readonly Timer mTimer = new(1f, (t) => CheckClear());

        private void Update()
        {
            _floweringTimer.Tick();
            if (_seed != null && !_floweringTimer.IsStart && !_seed.activeSelf)
            {
                _floweringTimer.Start();
            }

            if (_instances.First() != this)
            {
                return;
            }

            mTimer.Tick();
        }

        private void OnDisable()
        {
            _instances.Remove(this);
        }
    }
}