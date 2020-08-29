using System;
using HashCodeUtility;

namespace LosslessStitcher.Data
{
    public struct Range
        : IEquatable<Range>
    {
        public static Range Empty { get; } = new Range(0, 0);

        public int Start { get; }

        public int Stop { get; }

        public int Count => (Stop > Start) ? (Stop - Start) : 0;

        public bool IsEmpty => (Stop <= Start);

        public Range(int start, int stop)
        {
            Start = start;
            Stop = stop;
        }

        public void ForEach(Action<int> func)
        {
            for (int k = Start; k < Stop; ++k)
            {
                func(k);
            }
        }

        public override int GetHashCode()
        {
            return HashCodeBuilder.ForType<Range>().Ingest(Start, Stop).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Range other)
            {
                return this == other;
            }
            return false;
        }

        public bool Equals(Range other)
        {
            return this == other;
        }

        public static bool operator ==(Range r1, Range r2)
        {
            return r1.Start == r2.Start && r1.Stop == r2.Stop;
        }

        public static bool operator !=(Range r1, Range r2)
        {
            return r1.Start != r2.Start || r1.Stop != r2.Stop;
        }

        public bool Contains(int value)
        {
            return (value >= Start && value < Stop);
        }

        public override string ToString()
        {
            if (IsEmpty)
            {
                return "(Range empty)";
            }
            return $"(Range {Start} .. {Stop})";
        }
    }
}
