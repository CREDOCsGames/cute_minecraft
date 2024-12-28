using System;
namespace Puzzle
{
    public interface IDataLink
    {
        public event Action<byte[]> OnInteraction;
        public IMediatorInstance Mediator { get; set; }
    }

    public class FailLink : IDataLink
    {
        public IMediatorInstance Mediator { get; set; }

        public event Action<byte[]> OnInteraction;
    }

}
