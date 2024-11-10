namespace Puzzle
{
    public interface IPresentation<T>
    {
        public void UpstreamData(T elements, byte data);
    }

}

