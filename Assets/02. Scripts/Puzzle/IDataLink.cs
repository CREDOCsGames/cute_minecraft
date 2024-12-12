using System;
namespace Puzzle
{
    public interface IDataLink
    {
        public event Action<byte[]> OnInteraction;
        public NW.IMediatorInstance Mediator { get; set; }
    }

}
