using System;
using System.Collections;
using System.Collections.Generic;

namespace LosslessStitcher.Spatial.Internals
{
    using LosslessStitcher.Data;

    public static class AxisSubdiv
    {
        /// <summary>
        /// <see cref="MinAlign"/> implements <see cref="IAxisSubdiv"/> with the assumption that 
        /// all subdivisions, except the last one, have the same length. 
        /// 
        /// <para>
        /// The name of the class refers to the fact that the start / stop of each subdivision
        /// repeats uniformly, and that the start of the input range at zero <c>(0)</c> is 
        /// among one of those.
        /// </para>
        /// 
        /// </summary>
        /// 
        /// <inheritdoc cref="IAxisSubdiv"/>
        /// 
        public struct MinAlign
            : IAxisSubdiv
        {
            public int InputLength { get; }

            public Range InputRange => new Range(start: 0, stop: InputLength);

            public int MinLength { get; }

            public int MaxLength { get; }

            public int Count { get; }

            public Range this[int index]
            {
                get
                {
                    if (index < 0 || index >= Count)
                    {
                        throw new ArgumentOutOfRangeException(nameof(index));
                    }
                    int start = MaxLength * index;
                    int stop = Math.Min(start + MaxLength, InputLength);
                    return new Range(start: start, stop: stop);
                }
            }

            public MinAlign(int inputLength, int maxLength)
            {
                if (inputLength <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(inputLength));
                }
                if (maxLength <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(maxLength));
                }
                if (maxLength > inputLength)
                {
                    maxLength = inputLength;
                }
                InputLength = inputLength;
                MaxLength = maxLength;
                int quotient = checked((int)((uint)inputLength / (uint)maxLength));
                int remainder = checked((int)((uint)inputLength % (uint)maxLength));
                if (remainder == 0)
                {
                    MinLength = maxLength;
                    Count = quotient;
                }
                else
                {
                    MinLength = remainder;
                    Count = quotient + 1;
                }
            }

            public int Find(int input)
            {
                if (!TryFind(input, out int rangeIndex))
                {
                    throw new ArgumentOutOfRangeException(nameof(input));
                }
                return rangeIndex;
            }

            public bool TryFind(int input, out int rangeIndex)
            {
                if (input < 0 || input >= InputLength)
                {
                    rangeIndex = default;
                    return false;
                }
                rangeIndex = checked((int)((uint)input / (uint)MaxLength));
                return true;
            }

            public IEnumerator<Range> GetEnumerator()
            {
                for (int rangeIndex = 0; rangeIndex < Count; ++rangeIndex)
                {
                    yield return this[rangeIndex];
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
