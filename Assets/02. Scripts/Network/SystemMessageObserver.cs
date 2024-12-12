using System;

namespace NW
{
    public class SystemMessageObserver : IInstance
    {
        public DataReader DataReader { get; private set; } = new SystemReader();
        public event Action<byte[]> RecieveSystemMessage;
        public void InstreamData(byte[] data)
        {
            RecieveSystemMessage.Invoke(data);
        }
        public void SetMediator(IMediatorInstance mediator)
        {
        }
    }
}
