#if UNITY_EDITOR
using UnityEditor;
#endif
using Puzzle;
using UnityEngine;
using UnityEngine.Events;
using Util;
using System;

namespace Flow
{
    public class AreaComponent : InstancesMonobehaviour<AreaComponent>
    {
        public static event Action<Area> OnAreaEnter;
        public static event Action<Area> OnAreaExit;
        public static event Action<Area> OnAreaClaer;

        public static readonly Vector3Int[] Dirs =
        {
            new(1, 0, 0), new(-1, 0, 0),
            new(0, 0, 1), new(0, 0, -1), new(1, 0, 1),
            new(1, 0, -1), new(-1, 0, 1), new(-1, 0, -1),
            new(0, 1, 0), new(1, 1, 0), new(-1, 1, 0),
            new(0, 1, 1), new(0, 1, -1), new(1, 1, 1),
            new(1, 1, -1), new(-1, 1, 1), new(-1, 1, -1),
            new(0, -1, 0), new(1, -1, 0), new(-1, -1, 0),
            new(0, -1, 1), new(0, -1, -1), new(1, -1, 1),
            new(1, -1, -1), new(-1, -1, 1), new(-1, -1, -1)
        };

        [Header("[Extents of at least 5]")]
        [SerializeField] private Bounds _range;

        public Bounds Range
        {
            get
            {
                var runtimeRange = _range;
                runtimeRange.center = transform.position + _range.center;
                return runtimeRange;
            }
        }

        public Bounds HalfRange
        {
            get
            {
                var half = Range;
                half.extents /= 2;
                return half;
            }
        }

        [Header("[Options]")] public UnityEvent<Bounds> OnEnterEvent;
        public UnityEvent<Bounds> OnExitEvent;
        public UnityEvent<Bounds> OnClearEvent;

        public Area Area => GameManager.PuzzleArea;

        public void OnEnter()
        {
            Area.OnEnterEvent += InvokeEnterEvent;
            Area.OnExitEvent += InvokeExitEvnet;
            Area.OnClearEvent += InvokeClearEvent;
            Area.OnEnter();
        }

        public void OnExit()
        {
            Area.OnExit();
            Area.OnEnterEvent -= InvokeEnterEvent;
            Area.OnExitEvent -= InvokeExitEvnet;
            Area.OnClearEvent -= InvokeClearEvent;
        }

        public void OnClear()
        {
            Area.OnClear();
        }

        private void InvokeEnterEvent(Bounds bounds)
        {
            OnEnterEvent.Invoke(bounds);
        }

        private void InvokeExitEvnet(Bounds bounds)
        {
            OnExitEvent.Invoke(bounds);
        }

        private void InvokeClearEvent(Bounds bounds)
        {
            OnClearEvent.Invoke(bounds);
        }

#if UNITY_EDITOR
        [Header("[Debug]")][SerializeField] private bool _useViewAreaRange;
        [SerializeField] private bool _useViewBridgeRange;
        private void OnDrawGizmosSelected()
        {
            if (Application.isPlaying)
            {
                Gizmos.DrawWireCube(_range.center, Range.extents);
            }
            else
            {
                Gizmos.DrawWireCube(transform.position + _range.center, _range.extents);
            }

            if (Application.isPlaying)
            {
                return;
            }

            var originColor = Gizmos.color;
            var style = new GUIStyle();

            if (_useViewAreaRange)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(AreaManager.GetAreaNum(Range.center) * AreaManager.AreaRange,
                    Vector3.one * AreaManager.AreaRange);
                style.normal.textColor = Color.yellow;
                Handles.Label(AreaManager.GetAreaNum(Range.center) * AreaManager.AreaRange, "Area", style);
            }

            if (!_useViewBridgeRange)
            {
                return;
            }

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(Range.center + Vector3.up * Range.extents.y / 2,
                AreaManager.BridgeLimitDistance + Mathf.Max(Range.extents.x, Range.extents.z));
            Gizmos.color = originColor;
            style.normal.textColor = Color.blue;
            Handles.Label(transform.position, "Bridge Range", style);
        }
#endif
    }
}