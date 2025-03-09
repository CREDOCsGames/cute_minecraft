namespace Puzzle
{
    public class CoreSetting
    {
        public bool IsStart { get; private set; }
        public void StopCore(Face face)
        {
            IsStart = false;
        }
        public void StartCore(Face face)
        {
            IsStart = true;
        }
    }
}


