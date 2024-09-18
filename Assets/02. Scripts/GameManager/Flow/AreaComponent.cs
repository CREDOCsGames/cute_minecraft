using PlatformGame.Util;
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
        public Bounds Range => mRange;
        public Bounds HalfRange { get; private set; }

        [Header("[Options]")]
        public UnityEvent<Bounds> OnEnterEvent;
        public UnityEvent<Bounds> OnExitEvent;
        public UnityEvent<Bounds> OnClearEvent;

        Area mArea => GameManager.PuzzleArea;

        public void OnEnter()
        {
            mArea.Range = Range;
            mArea.OnEnterEvent += (area) => OnEnterEvent.Invoke(area);
            mArea.OnExitEvent += (area) => OnExitEvent.Invoke(area);
            mArea.OnClearEvent += (area) => OnClearEvent.Invoke(area);
            mArea.OnEnter();
        }

        public void OnExit()
        {
            mArea.OnExit();
            mArea.OnEnterEvent -= (area) => OnEnterEvent.Invoke(area);
            mArea.OnExitEvent -= (area) => OnExitEvent.Invoke(area);
            mArea.OnClearEvent -= (area) => OnClearEvent.Invoke(area);
            mArea.Range = Area.zero;
        }

        public void OnClear()
        {
            mArea.OnClear();
        }

        void Awake()
        {
            mRange.center = transform.position + mRange.center;
            var half = Range;
            half.extents /= 2;
            HalfRange = half;
        }

#if UNITY_EDITOR
        [SerializeField] int AreaRange = 10;
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
            Gizmos.color = Color.yellow;
            AreaManager.AreaRange = AreaRange;
            Gizmos.DrawWireCube(AreaManager.GetSectorNum(transform.position + Range.center) * AreaRange, Vector3.one * AreaRange);
            Gizmos.color = originColor;
        }
#endif
    }

}
