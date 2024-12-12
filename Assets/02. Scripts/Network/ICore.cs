using System;

namespace NW
{
    public interface ICore
    {
        public void SetMediator(IMediatorCore mediator);
        public void InstreamData(byte[] data);
        public DataReader DataReader { get; }
    }

    public abstract class LocalCore : ICore
    {
        public DataReader DataReader => throw new NotImplementedException();

        public void InstreamData(byte[] data)
        {
            throw new NotImplementedException();
        }

        public void SetMediator(IMediatorCore mediator)
        {
            throw new NotImplementedException();
        }
    }

}
