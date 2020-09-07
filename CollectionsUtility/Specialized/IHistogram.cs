using System.Collections.Generic;

namespace CollectionsUtility.Specialized
{
    using CollectionsUtility.Specialized.Internals;

    /// <summary>
    /// Histogram.
    /// </summary>
    /// 
    /// <typeparam name="TKey">
    /// The key type.
    /// </typeparam>
    /// 
    /// <typeparam name="TBin">
    /// The quantity associated with each key. This quantity can be integer-valued or real-valued.
    /// </typeparam>
    /// 
    public interface IHistogram<TKey, TBin>
        : IReadOnlyDictionary<TKey, TBin>
        where TBin : struct
    {
        /// <summary>
        /// Basic arithmetics operating on the histogram bin value type.
        /// </summary>
        IHistArith<TBin> HistArith { get; }

        TBin DefaultIncrement { get; set; }

        void Add(TKey key);

        void Add(TKey key, TBin amount);

        void Add(KeyValuePair<TKey, TBin> keyAndAmount);
    }
}
