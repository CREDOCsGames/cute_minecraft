namespace Puzzle
{
    public interface IDataChecker
    {
        public bool Equals(byte[] data);
    }
    public class ByteChecker : IDataChecker
    {
        private readonly byte[] _data;
        public ByteChecker(byte[] data)
        {
            _data = data;
        }
        public bool Equals(byte[] data)
        {
            return _data.Equals(data);
        }
    }
    public class CountChecker : IDataChecker
    {
        private readonly byte _length;
        public CountChecker(byte length)
        {
            _length = length;
        }
        public bool Equals(byte[] data)
        {
            return _length== data.Length;
        }
    }

}

