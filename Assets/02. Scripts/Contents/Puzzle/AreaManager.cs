using PlatformGame.Contents.Puzzle;
using PlatformGame.Manager;
using PlatformGame.Util;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AreaManager
{
    static Dictionary<Vector3Int, AreaComponent> mSector { get; set; } = new();

    static public int AreaRange = 40;
    static public float BridgeLimitDistance = 10f;

    static List<Bridge> mActiveBridges = new();
    public static void Connect()
    {
        var basis = GameManager.PuzzleArea;
        if (basis == null)
        {
            return;
        }

        var sectorNum = GetSectorNum(basis.Range.center);
        var nearByAreas = GetNearByAreas(sectorNum);

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
        mActiveBridges.ForEach(x => x.DisConnect());
    }

    public static void NumberingAreas()
    {
        mSector.Clear();
        var areas = InstancesMonobehaviour<AreaComponent>.Instances;
        if (!areas.Any())
        {
            return;
        }

        foreach (var area in areas)
        {
            var sectorNum = GetSectorNum(area.Range.center);
            if (mSector.ContainsKey(sectorNum))
            {
                Debug.Assert(false, $"The two areas overlapped :  {mSector[sectorNum].name},{area.name}");
                continue;
            }
            mSector.Add(sectorNum, area);
            area.enabled = false;
        }
    }

    public static List<AreaComponent> GetNearByAreas(Vector3Int sectorNum)
    {
        List<AreaComponent> nearbyAreas = new();

        foreach (var dir in AreaComponent.Dirs)
        {
            if (mSector.TryGetValue(sectorNum + dir, out var area))
            {
                nearbyAreas.Add(area);
            }
        }

        return nearbyAreas;
    }

    public static bool TryGetArea(Vector3Int sectorNum, out AreaComponent area)
    {
        return mSector.TryGetValue(sectorNum, out area);
    }

    // class Sector
    public static Vector3Int GetSectorNum(Vector3 position)
    {
        Vector3Int sectorNum = Vector3Int.zero;
        sectorNum.x = GetSectorAxis(position.x);
        sectorNum.y = GetSectorAxis(position.y);
        sectorNum.z = GetSectorAxis(position.z);
        return sectorNum;
    }

    static int GetSectorAxis(float value)
    {
        int offset = Mathf.Max((int)Mathf.Abs(value) - AreaRange / 2, 0);
        return (offset == 0 ? 0 : 1 + offset / AreaRange)
                        * (value > 0 ? 1 : -1);
    }
    //
}