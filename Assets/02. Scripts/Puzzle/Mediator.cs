namespace Puzzle
{
    public interface IMediatorCore
    {
        public void InstreamDataCore<T>(byte[] data) where T : DataReader;
    }

    public interface IMediatorInstance
    {
        public void InstreamDataInstance<T>(byte[] data) where T : DataReader;
    }
}