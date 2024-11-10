using System;

namespace Puzzle
{
    public interface IDataLink<T>
    {
        public event Action<byte[]> OnInteraction;
        public void Link(T t, byte[] data);
    }

}
