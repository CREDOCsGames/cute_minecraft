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
        readonly IndexInt mIndex;
        public IndexIntClamp(int min = int.MinValue, int max = int.MaxValue)
        {
            mIndex = new IndexInt(min, max);
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
            if (mIndex.Value == mIndex.Min)
            {
                return;
            }
            mIndex.Prev();
        }
    }

    public class IndexIntEvent : IIndexInt
    {
        readonly IIndexInt mIndex;
        readonly Action<int> mEvent;

        public int Value => mIndex.Value;

        public IndexIntEvent(IIndexInt index, Action<int> @event)
        {
            mIndex = index;
            mEvent = @event;
        }

        public void Next()
        {
            mIndex.Next();
            mEvent.Invoke(Value);
        }

        public void Prev()
        {
            mIndex.Prev();
            mEvent.Invoke(Value);
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
        readonly IndexByte mIndex;
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
        public byte Value => mIndex.Value;
        readonly IIndexByte mIndex;
        readonly Action<byte> mEvent;

        public IndexByteEvent(IIndexByte index, Action<byte> @event)
        {
            mIndex = index;
            mEvent = @event;
        }

        public void Next()
        {
            mIndex.Next();
            mEvent.Invoke(Value);
        }

        public void Prev()
        {
            mIndex.Prev();
            mEvent.Invoke(Value);
        }
    }
}
