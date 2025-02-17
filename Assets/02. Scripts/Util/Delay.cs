using UnityEngine;

namespace Util
{
    public interface IDelay
    {
        public Delay Delay { get; }
    }

    public class Delay
    {
        public float Duration { get; set; }
        public float StartTime;
        public Delay(float duration)
        {
            Duration = duration;
        }
        public void DoStart()
        {
            StartTime = Time.time;
        }
        public bool IsDelay()
        {
            return Time.time <= StartTime + Duration;
        }
    }

}