using System;

namespace Puzzle
{
    public class MessageObserver : IInstance
    {
        public DataReader DataReader { get; private set; }
        public MessageObserver(DataReader dataReader)
        {
            DataReader = dataReader;
        }
        public event Action<byte[]> RecieveSystemMessage;
        public void InstreamData(byte[] data)
        {
            RecieveSystemMessage.Invoke(data);
        }
        public void SetMediator(IMediatorInstance mediator)
        {
        }
    }
    public class MessageObserverCore : ICore
    {
        public DataReader DataReader { get; private set; }
        public MessageObserverCore(DataReader dataReader)
        {
            DataReader = dataReader;
        }
        public event Action<byte[]> RecieveSystemMessage;
        public void InstreamData(byte[] data)
        {
            RecieveSystemMessage.Invoke(data);
        }
        public void SetMediator(IMediatorCore mediator)
        {
        }
    }

}
