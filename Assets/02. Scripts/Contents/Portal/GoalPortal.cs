using PlatformGame.Contents.Loader;
using System.Linq;
using UnityEngine;

namespace PlatformGame.Contents
{
    public class GoalPortal : Portal
    {
        public LoaderType LoaderType;
        [Range(0, byte.MaxValue)]
        public byte NeedCharacters = 1;
        public float LoadDelay;

        protected override bool CanRunningPortal(Character.Character other)
        {
            Debug.Assert(NeedCharacters <= mEntrylist.Count);
            return State == WorkState.Ready &&
                   NeedCharacters <= mEntrylist.Count(x => x.Value);
        }

        protected override void RunPortal()
        {
            base.RunPortal();
            Contents.Instance.SetLoaderType(LoaderType);
            GameManager.Instance.LoadGame(LoadDelay);
        }
    }
}