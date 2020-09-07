using System;
using System.Collections;
using System.Collections.Generic;

namespace CollectionsUtility.Specialized
{
    using CollectionsUtility.Specialized.Internals;
    using System.Collections.ObjectModel;

    public class Histogram<TKey, TBin>
        : IHistogram<TKey, TBin>
        where TBin : struct
    {
        #region private 
        private UniqueList<TKey> _mapping;
        private List<TBin> _bins;
        #endregion

        public IHistArith<TBin> HistArith { get; }

        public IEnumerable<TKey> Keys { get; }

        public IEnumerable<TBin> Values { get; }

        public int Count => _mapping.Count;

        public TBin DefaultIncrement { get; set; }

        public TBin this[TKey key] => _bins[_mapping.IndexOf(key)];

        public Histogram(IHistArith<TBin> histArith)
        {
            HistArith = histArith;
            _mapping = new UniqueList<TKey>();
            _bins = new List<TBin>();
            DefaultIncrement = HistArith.One;
            Keys = _mapping;
            Values = _bins;
        }

        public void Add(TKey key)
        {
            Add(key, DefaultIncrement);
        }

        public void Add(KeyValuePair<TKey, TBin> keyAndAmount)
        {
            Add(keyAndAmount.Key, keyAndAmount.Value);
        }

        public void Add(TKey key, TBin amount)
        {
            int index = _mapping.Add(key);
            while (index >= _bins.Count)
            {
                _bins.Add(HistArith.Zero);
            }
            _bins[index] = HistArith.Add(_bins[index], amount);
        }

        public IEnumerator<KeyValuePair<TKey, TBin>> GetEnumerator()
        {
            foreach (var indexAndKey in (IEnumerable<(int Index, TKey Item)>)_mapping)
            {
                yield return new KeyValuePair<TKey, TBin>(indexAndKey.Item, _bins[indexAndKey.Index]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool ContainsKey(TKey key)
        {
            return _mapping.IndexOf(key) >= 0;
        }

        public bool TryGetValue(TKey key, out TBin value)
        {
            int index = _mapping.IndexOf(key);
            if (index >= 0)
            {
                value = _bins[index];
                return true;
            }
            value = default;
            return false;
        }
    }
}
