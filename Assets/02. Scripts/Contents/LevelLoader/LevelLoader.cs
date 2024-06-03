using UnityEngine;

namespace PlatformGame.Contents.Loader
{
    public class LevelLoader : ILevelLoader
    {
        public WorkState State
        {
            get { return mEndTime <= Time.time ? WorkState.Ready : WorkState.Action; }
        }

        float mEndTime;

        public void LoadNext()
        {
            mEndTime = Time.time + 3f;
        }
    }
}