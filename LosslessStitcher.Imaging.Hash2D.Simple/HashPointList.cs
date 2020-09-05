using System;
using System.Collections.Generic;

namespace LosslessStitcher.Imaging.Hash2D.Simple
{
    using LosslessStitcher.Data;
    using System.Collections;

    public class HashPointList
        : IHashPointList
    {
        public HashPointSortKey SortKey { get; }

        public int Count { get; private set; }

        public IReadOnlyList<Point> Points { get; }

        public IReadOnlyList<int> HashValues { get; }

        public (Point Point, int HashValue) this[int index] => (Points[index], HashValues[index]);

        public HashPointList(IReadOnlyList<Point> points, IReadOnlyList<int> hashValues, HashPointSortKey sortKey)
        {
            if (points is null)
            {
                throw new ArgumentNullException(nameof(points));
            }
            if (hashValues is null)
            {
                throw new ArgumentNullException(nameof(hashValues));
            }
            Count = points.Count;
            if (hashValues.Count != Count)
            {
                throw new ArgumentException(nameof(hashValues));
            }
            Points = points;
            HashValues = hashValues;
            SortKey = sortKey;
        }

        public IEnumerator<(Point Point, int HashValue)> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public class Enumerator
            : IEnumerator<(Point Point, int HashValue)>
        {
            private HashPointList _host;
            private int _index;

            public (Point Point, int HashValue) Current
            {
                get => (_host.Points[_index], _host.HashValues[_index]);
            }

            object IEnumerator.Current => Current;

            public Enumerator(HashPointList host)
            {
                if (host is null)
                {
                    throw new ArgumentNullException(nameof(host));
                }
                _host = host;
                _index = -1;
            }

            public bool MoveNext()
            {
                if (_index < 0)
                {
                    _index = 0;
                    return (_index < _host.Count);
                }
                if (_index + 1 < _host.Count)
                {
                    ++_index;
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                _index = -1;
            }

            public void Dispose()
            {
            }
        }
    }
}
