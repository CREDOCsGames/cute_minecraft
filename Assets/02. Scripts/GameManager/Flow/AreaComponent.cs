using PlatformGame.Util;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame.Manager
{
    public class AreaComponent : InstancesMonobehaviour<AreaComponent>
    {
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
        [SerializeField] Bounds mRange;
        public Bounds Range
        {
            get
            {
                var runtimeRange = mRange;
                runtimeRange.center = transform.position + mRange.center;
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

        [Header("[Options]")]
        public UnityEvent<Bounds> OnEnterEvent;
        public UnityEvent<Bounds> OnExitEvent;
        public UnityEvent<Bounds> OnClearEvent;

        Area mArea => GameManager.PuzzleArea;

        public void OnEnter()
        {
            mArea.Range = Range;
            mArea.OnEnterEvent += InvokeEnterEvent;
            mArea.OnExitEvent += InvokeExitEvnet;
            mArea.OnClearEvent += InvokeClearEvent;
            mArea.OnEnter();
        }

        public void OnExit()
        {
            mArea.OnExit();
            mArea.OnEnterEvent -= InvokeEnterEvent;
            mArea.OnExitEvent -= InvokeExitEvnet;
            mArea.OnClearEvent -= InvokeClearEvent;
            mArea.Range = Area.zero;
        }

        public void OnClear()
        {
            mArea.OnClear();
        }

        void InvokeEnterEvent(Bounds bounds)
        {
            OnEnterEvent.Invoke(bounds);
        }

        void InvokeExitEvnet(Bounds bounds)
        {
            OnExitEvent.Invoke(bounds);
        }

        void InvokeClearEvent(Bounds bounds)
        {
            OnClearEvent.Invoke(bounds);
        }

#if UNITY_EDITOR
        [Header("[Debug]")]
        [SerializeField] bool UseViewAreaRange;
        [SerializeField] bool UseViewBridgeRange;
        void OnDrawGizmosSelected()
        {
            if (Application.isPlaying)
            {
                Gizmos.DrawWireCube(mRange.center, Range.extents);
            }
            else
            {
                Gizmos.DrawWireCube(transform.position + mRange.center, mRange.extents);
            }

            if (Application.isPlaying)
            {
                return;
            }

            var originColor = Gizmos.color;
            var style = new GUIStyle();

            if (UseViewAreaRange)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(AreaManager.GetSectorNum(Range.center) * AreaManager.AreaRange, Vector3.one * AreaManager.AreaRange);
                style.normal.textColor = Color.yellow;
                Handles.Label(AreaManager.GetSectorNum(Range.center) * AreaManager.AreaRange, "Area", style);
            }

            if (UseViewBridgeRange)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(Range.center + Vector3.up * Range.extents.y / 2, AreaManager.BridgeLimitDistance + Mathf.Max(Range.extents.x, Range.extents.z));
                Gizmos.color = originColor;
                style.normal.textColor = Color.blue;
                Handles.Label(transform.position, "Bridge Range", style);
            }
        }
#endif
    }

}
