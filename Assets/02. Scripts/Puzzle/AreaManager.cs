using Flow;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util;

namespace Puzzle
{
    public static class AreaManager
    {
        static Dictionary<Vector3Int, AreaComponent> mAreas { get; set; } = new();

        public static int AreaRange = 40;
        public static float BridgeLimitDistance = 10f;

        static readonly List<Bridge> mActiveBridges = new();

        public static void Connect()
        {
            var basis = GameManager.PuzzleArea;
            if (basis == null)
            {
                return;
            }

            var areaNum = GetAreaNum(basis.Range.center);
            var nearByAreas = GetNearByAreas(areaNum);

            foreach (var area in nearByAreas)
            {
                if (basis.Range.Contains(area.Range.center))
                {
                    continue;
                }

                var bridge = new Bridge(BridgeLimitDistance);
                bridge.SetConnectionPoints(basis.Range, area.Range);
                bridge.ConnectAtoB();
                mActiveBridges.Add(bridge);
            }
        }

        public static void DisConnect()
        {
            mActiveBridges.ForEach(x => x.Disconnect());
        }

        public static void NumberingAreas()
        {
            mAreas.Clear();
            var areas = InstancesMonobehaviour<AreaComponent>.Instances;
            if (!areas.Any())
            {
                return;
            }

            foreach (var area in areas)
            {
                var areaNum = GetAreaNum(area.Range.center);
                if (!mAreas.TryAdd(areaNum, area))
                {
                    Debug.Assert(false, $"The two areas overlapped :  {mAreas[areaNum].name},{area.name}");
                    continue;
                }

                area.enabled = false;
            }
        }

        public static List<AreaComponent> GetNearByAreas(Vector3Int sectorNum)
        {
            List<AreaComponent> nearbyAreas = new();

            foreach (var dir in AreaComponent.Dirs)
            {
                if (mAreas.TryGetValue(sectorNum + dir, out var area))
                {
                    nearbyAreas.Add(area);
                }
            }

            return nearbyAreas;
        }

        public static bool TryGetArea(Vector3Int sectorNum, out AreaComponent area)
        {
            return mAreas.TryGetValue(sectorNum, out area);
        }

        public static Vector3Int GetAreaNum(Vector3 position)
        {
            var areaNum = Vector3Int.zero;
            areaNum.x = GetSectorAxis(position.x);
            areaNum.y = GetSectorAxis(position.y);
            areaNum.z = GetSectorAxis(position.z);
            return areaNum;
        }

        static int GetSectorAxis(float value)
        {
            var offset = Mathf.Max((int)Mathf.Abs(value) - AreaRange / 2, 0);
            return (offset == 0 ? 0 : 1 + offset / AreaRange)
                   * (value > 0 ? 1 : -1);
        }
    }
}