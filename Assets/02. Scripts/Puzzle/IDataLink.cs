using System;
namespace Puzzle
{
    public interface IDataLink
    {
        public IMediatorInstance Mediator { get; set; }
    }

    public class FailLink : IDataLink
    {
        public IMediatorInstance Mediator { get; set; }

    }

}
