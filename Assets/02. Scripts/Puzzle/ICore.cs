using System;

namespace Puzzle
{
    public interface ICore
    {
        public void InstreamData(byte[] data);
        public event Action<byte[]> InstreamEvent;
        public void Init();
    }

}