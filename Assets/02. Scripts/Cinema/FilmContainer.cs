using Cinema;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

public class FilmContainer
{
    private static Dictionary<string, Film> _chach = new();
    public static Film Search(string key)
    {
        if (!_chach.ContainsKey(key))
        {
            var timelines = Resources.LoadAll<TimelineAsset>("Timeline").ToList();
            var index = timelines.FindIndex(timeline => key.Equals(timeline.name));
            if (index == -1)
            {
                _chach.Add(key, Film.EMPTY);
            }
            else
            {
                _chach.Add(key, FilmFactory.MakeFilmAs(timelines[index]));
            }
        }
        return _chach[key];
    }
    public static void ClearChach()
    {
        _chach.Clear();
    }
}
