namespace PlatformGame.Contents.Loader
{
    public interface ILevelLoader
    {
        public void LoadNext();
        public WorkState State { get; }
    }
}