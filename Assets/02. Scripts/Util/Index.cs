using System;

namespace Util
{
    public interface IIndexInt
    {
        public void Next();
        public void Prev();
        public int Value { get; }
    }

    public class IndexInt : IIndexInt
    {
        public IndexInt(int min = int.MinValue, int max = int.MaxValue)
        {
            Min = min;
            Max = max;
            Value = min;
        }
        public int Min { get; private set; }
        public int Max { get; private set; }
        public int Value { get; private set; }
        public void Next()
        {
            Value++;
            Value = Max < Value ? Min : Value;
        }
        public void Prev()
        {
            Value--;
            Value = Value < Min ? Max : Value;
        }
    }

    public class IndexIntClamp : IIndexInt
    {
        public int Value { get; private set; }
        private readonly IndexInt _index;
        public IndexIntClamp(int min = int.MinValue, int max = int.MaxValue)
        {
            _index = new IndexInt(min, max);
        }
        public void Next()
        {
            if (_index.Value == _index.Max)
            {
                return;
            }
            _index.Next();
        }
        public void Prev()
        {
            if (_index.Value == _index.Min)
            {
                return;
            }
            _index.Prev();
        }
    }

    public class IndexIntEvent : IIndexInt
    {
        private readonly IIndexInt _index;
        private readonly Action<int> _event;

        public int Value => _index.Value;

        public IndexIntEvent(IIndexInt index, Action<int> @event)
        {
            _index = index;
            _event = @event;
        }

        public void Next()
        {
            _index.Next();
            _event.Invoke(Value);
        }

        public void Prev()
        {
            _index.Prev();
            _event.Invoke(Value);
        }
    }

    public interface IIndexByte
    {
        public void Next();
        public void Prev();
        public byte Value { get; }
    }

    public class IndexByte : IIndexByte
    {
        public byte Max { get; private set; }
        public byte Min { get; private set; }
        public byte Value { get; private set; }
        public IndexByte(byte min = byte.MinValue, byte max = byte.MaxValue)
        {
            Min = min;
            Max = max;
        }
        public void Next()
        {
            Value = Value == Max ? Min : (byte)(Value + 1);
        }
        public void Prev()
        {
            Value = Value == Min ? Max : (byte)(Value - 1);
        }
    }
    public class IndexByteClamp : IIndexByte
    {
        public byte Value { get; private set; }
        private readonly IndexByte mIndex;
        public IndexByteClamp(byte min = byte.MinValue, byte max = byte.MaxValue)
        {
            mIndex = new IndexByte(min, max);
        }
        public void Next()
        {
            if (mIndex.Value == mIndex.Max)
            {
                return;
            }
            mIndex.Next();
        }
        public void Prev()
        {
            if (mIndex.Value == byte.MinValue)
            {
                return;
            }
            mIndex.Prev();
        }
    }
    public class IndexByteEvent : IIndexByte
    {
        public byte Value => _index.Value;
        private readonly IIndexByte _index;
        private readonly Action<byte> _event;

        public IndexByteEvent(IIndexByte index, Action<byte> @event)
        {
            _index = index;
            _event = @event;
        }

        public void Next()
        {
            _index.Next();
            _event.Invoke(Value);
        }

        public void Prev()
        {
            _index.Prev();
            _event.Invoke(Value);
        }
    }
}
