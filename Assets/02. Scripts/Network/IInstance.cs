namespace NW
{
    public interface IInstance
    {
        public void SetMediator(IMediatorInstance mediator);
        public void InstreamData(byte[] data);
        public DataReader DataReader { get; }
    }

}
