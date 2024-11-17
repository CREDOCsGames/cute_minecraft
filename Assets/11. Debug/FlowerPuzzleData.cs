using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Data/FlowerPuzzleData")]
public class FlowerPuzzleData : ScriptableObject
{
    public string UseClearThanLock
    {
        get
        {
            return PropertyFile.Get(nameof(UseClearThanLock), "False");
        }
        set
        {
            PropertyFile.Set(nameof(UseClearThanLock), value);
        }
    }
    public string UseClearConditionThanBaseFace
    {
        get
        {
            return PropertyFile.Get(nameof(UseClearConditionThanBaseFace), "False");
        }
        set
        {
            PropertyFile.Set(nameof(UseClearConditionThanBaseFace), value);
        }
    }

    public string UseLinkedClear
    {
        get
        {
            return PropertyFile.Get(nameof(UseLinkedClear), "False");
        }
        set
        {
            PropertyFile.Set(nameof(UseLinkedClear), value);
        }
    }

    public string UseMinimap
    {
        get
        {
            return PropertyFile.Get(nameof(UseMinimap), "False");
        }
        set
        {
            PropertyFile.Set(nameof(UseMinimap), value);
        }
    }

    public List<List<int>> Links
    {
        get
        {
            var list = new List<List<int>>
            {
                new List<int>(),
                new List<int>(),
                new List<int>(),
                new List<int>(),
                new List<int>(),
                new List<int>()
            };

            for (int i = 0; i < LinkData.Length; i++)
            {
                if (LinkData[i] == 'x')
                {
                    continue;
                }
                var index = i / 6;
                var num = i % 6;
                list[index].Add(num);

            }

            return list;

        }
    }
    public string LinkData
    {
        get
        {
            return PropertyFile.Get(nameof(LinkData), "oxxxxxxoxxxxxxoxxxxxxoxxxxxxoxxxxxxo");
        }
        set
        {
            PropertyFile.Set(nameof(LinkData), value);
        }
    }

    public void LinkFaces(int data)
    {
        var arr = LinkData.ToCharArray();
        arr[data] = 'o';
        LinkData = new string(arr);
    }

    public void UnLinkFaces(int data)
    {
        var arr = LinkData.ToCharArray();
        arr[data] = 'x';
        LinkData = new string(arr);
    }

    public void ToggleLock(bool toggle)
    {
        UseClearThanLock = toggle.ToString();
    }

    public void ToggleBase(bool toggle)
    {
        UseClearConditionThanBaseFace = toggle.ToString();
    }

    public void ToggleLinkedClear(bool toggle)
    {
        UseLinkedClear = toggle.ToString();
    }

    public void ToggleMinimap(bool toggle)
    {
        UseMinimap = toggle.ToString();
    }

}

public static class IOStream
{
    public static void WriteAllText(string path, string contents)
    {
        try
        {
            File.WriteAllText(path, contents);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    public static string ReadAllText(string path)
    {
        string contents = "";
        try
        {
            contents = File.ReadAllText(path);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }

        return contents;
    }
}

public static class PropertyFile
{
    public static string Get(string name, string @default)
    {
        var path = Path.Combine(Application.dataPath, name) + ".text";
        if (!File.Exists(path))
        {
            IOStream.WriteAllText(path, @default);
        }
        var data = IOStream.ReadAllText(path);
        return data;
    }

    public static void Set(string name, string value)
    {
        var path = Path.Combine(Application.dataPath, name) + ".text";
        IOStream.WriteAllText(path, value);
    }
}