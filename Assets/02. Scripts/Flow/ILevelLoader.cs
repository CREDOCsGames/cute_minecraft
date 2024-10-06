namespace Flow
{
    public interface ILevelLoader
    {
        public void LoadNext();
        public WorkState State { get; }
    }
}