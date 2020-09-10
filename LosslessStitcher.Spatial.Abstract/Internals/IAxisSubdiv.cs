using System.Collections.Generic;

namespace LosslessStitcher.Spatial.Internals
{
    using LosslessStitcher.Data;

    /// <summary>
    /// <see cref="IAxisSubdiv"/> represents a partitioning (subdivision) of a one-dimensional 
    /// input range into multiple non-overlapping ranges.
    /// 
    /// <para>
    /// Implementation notes.
    /// <br/>
    /// The following properties and methods are required by <see cref="IReadOnlyList{T}"/>:
    /// <br/>
    /// Count, this[int] =&gt; Range, GetEnumerator()
    /// </para>
    /// </summary>
    public interface IAxisSubdiv
        : IReadOnlyList<Range>
    {
        /// <summary>
        /// The input length.
        /// </summary>
        int InputLength { get; }

        /// <summary>
        /// The input range, which is required to be <c>Range(0, InputLength)</c>.
        /// </summary>
        Range InputRange { get; }

        /// <summary>
        /// The minimum length among the subdivisions.
        /// </summary>
        int MinLength { get; }

        /// <summary>
        /// The maximum length among the subdivisions.
        /// </summary>
        int MaxLength { get; }

        /// <summary>
        /// Given an integer in the input range, find the index of the subdivision which
        /// contains the input integer value.
        /// </summary>
        /// <param name="input">
        /// An integer value within the input range.
        /// </param>
        /// <returns>
        /// The range index.
        /// </returns>
        int Find(int input);

        /// <summary>
        /// Given an integer in the input range, find the index of the subdivision which
        /// contains the input integer value.
        /// 
        /// <para>
        /// If the input integer value is invalid, this method returns false.
        /// This method does not throw an exception even if the argument is invalid.
        /// </para>
        /// 
        /// </summary>
        /// <param name="input">
        /// An integer value within the input range.
        /// </param>
        /// <returns>
        /// The range index.
        /// </returns>
        /// 
        bool TryFind(int input, out int rangeIndex);
    }
}
