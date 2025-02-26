using System;

namespace Puzzle
{
    public class MessageObserverFromCore : IInstance
    {
        public DataReader DataReader { get; private set; }
        public MessageObserverFromCore(DataReader dataReader)
        {
            DataReader = dataReader;
        }
        public event Action<byte[]> OnRecieveMessage;
        public void InstreamData(byte[] data)
        {
            OnRecieveMessage.Invoke(data);
        }
        public void SetMediator(IMediatorInstance mediator)
        {
        }
    }
    public class MessageObserverFromInstance : ICore
    {
        public DataReader DataReader { get; private set; }
        public MessageObserverFromInstance(DataReader dataReader)
        {
            DataReader = dataReader;
        }
        public event Action<byte[]> OnRecieveMessage;
        public void InstreamData(byte[] data)
        {
            OnRecieveMessage.Invoke(data);
        }
        public void SetMediator(IMediatorCore mediator)
        {
        }
    }

}
