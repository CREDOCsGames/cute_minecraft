namespace PlatformGame.Contents
{
    public class GoalPortal : Portal
    {
        public LoaderType LoaderType;
        public float LoadDelay;

        protected override void RunPortal()
        {
            base.RunPortal();
            Contents.Instance.SetLoaderType(LoaderType);
            GameManager.Instance.LoadGame(LoadDelay);
        }
    }
}