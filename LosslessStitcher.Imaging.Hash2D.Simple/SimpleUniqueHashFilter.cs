using System;
using System.Collections.Generic;
using System.Text;

namespace LosslessStitcher.Imaging.Hash2D.Simple
{
    using LosslessStitcher.Data;

    public class SimpleUniqueHashFilter
        : IUniqueHashFilter
    {
        public IHashValueRangeFilter HashValueRangeFilter { get; }

        private int _filterRangeMin;

        private int _filterRangeMax;

        public SimpleUniqueHashFilter(double filterFrac)
        {
            HashValueRangeFilter = new HashValueRangeFilter(filterFrac);
            _filterRangeMin = HashValueRangeFilter.FilterRangeMin;
            _filterRangeMax = HashValueRangeFilter.FilterRangeMax;
        }

        public SimpleUniqueHashFilter(IHashValueRangeFilter hashValueRangeFilter)
        {
            HashValueRangeFilter = new HashValueRangeFilter(hashValueRangeFilter);
            _filterRangeMin = HashValueRangeFilter.FilterRangeMin;
            _filterRangeMax = HashValueRangeFilter.FilterRangeMax;
        }

        public IHashPointList Extract(IBitmapRowSource<int> hashedInput)
        {
            Size size = hashedInput.Size;
            var result = new ResultBuilder();
            var inputWrapper = new TwoRowSourceWrapper(hashedInput);
            for (int row = 0; row < size.Height; ++row)
            {
                inputWrapper.LoadRow(row);
                _ProcessRow(result, inputWrapper);
            }
            return result.ToHashPointList();
        }

        private void _ProcessRow(ResultBuilder result, TwoRowSourceWrapper inputWrapper)
        {
            Size size = inputWrapper.Size;
            int width = size.Width;
            int row = inputWrapper.CurrRow;
            int[] currArray = inputWrapper.CurrArray;
            int currOffset = inputWrapper.CurrOffset;
            int nextX = 0;
            while (nextX < width)
            {
                int startX = nextX;
                int hashValue = currArray[currOffset + startX];
                int stopX = startX + 1;
                while (stopX < width &&
                    currArray[currOffset + stopX] == hashValue)
                {
                    ++stopX;
                }
                _ProcessRun(result, inputWrapper, hashValue, startX, stopX);
                nextX = stopX;
            }
        }

        private void _ProcessRun(ResultBuilder result, TwoRowSourceWrapper inputWrapper, int hashValue, int startX, int stopX)
        {
            int row = inputWrapper.CurrRow;
            if (result._isUnique.TryGetValue(hashValue, out bool wasUnique))
            {
                if (wasUnique)
                {
                    result._isUnique[hashValue] = false;
                }
                return;
            }
            if (!_WithinFilterRange(hashValue))
            {
                return;
            }
            int runLength = stopX - startX;
            if (runLength > 1)
            {
                result._isUnique.Add(hashValue, false);
                return;
            }
            int prevRowX = _TryFindInPrevRow(inputWrapper, hashValue, startX, stopX);
            if (prevRowX >= 0)
            {
                result._isUnique.Add(hashValue, false);
                return;
            }
            result._isUnique.Add(hashValue, true);
            result._points.Add(new Point(startX, row));
            result._hashValues.Add(hashValue);
        }

        private int _TryFindInPrevRow(TwoRowSourceWrapper inputWrapper, int hashValue, int startX, int stopX)
        {
            Size size = inputWrapper.Size;
            if (inputWrapper.PrevRow < 0)
            {
                return -1;
            }
            if (startX - 1 >= 0)
            {
                --startX;
            }
            if (stopX + 1 < size.Width)
            {
                ++stopX;
            }
            int[] prevArray = inputWrapper.PrevArray;
            int prevOffset = inputWrapper.PrevOffset;
            for (int x = startX; x < stopX; ++x)
            {
                if (prevArray[prevOffset + x] == hashValue)
                {
                    return x;
                }
            }
            return -1;
        }

        private bool _WithinFilterRange(int hashValue)
        {
            return hashValue >= _filterRangeMin && 
                hashValue <= _filterRangeMax;
        }

        public class ResultBuilder
        {
            internal Dictionary<int, bool> _isUnique;
            internal List<Point> _points;
            internal List<int> _hashValues;

            public ResultBuilder()
            {
                _isUnique = new Dictionary<int, bool>();
                _points = new List<Point>();
                _hashValues = new List<int>();
            }

            public HashPointList ToHashPointList()
            {
                // ======
                // Hash points that are added to the list may be found to be a duplicate subsequently.
                // Therefore a final filtering is necessary.
                // ======
                // Multi-image hash point detections prefer the list sorted by hash value for easier
                // collation during merging.
                // ======
                // Also trim the final lists to exact size, since a HashPointList will be kept around 
                // for the entire duration of the tracking algorithm.
                // ======
                int unfilteredCount = _hashValues.Count;
                var goodList = new List<(int HashValue, Point Point)>();
                for (int index = 0; index < unfilteredCount; ++index)
                {
                    int hashValue = _hashValues[index];
                    if (!_isUnique[hashValue])
                    {
                        continue;
                    }
                    goodList.Add((hashValue, _points[index]));
                }
                int CompareHashPointFunc(
                    (int HashValue, Point Point) first,
                    (int HashValue, Point Point) second)
                {
                    if (first.HashValue < second.HashValue) return -1;
                    if (first.HashValue > second.HashValue) return 1;
                    if (first.Point.Y < second.Point.Y) return -1;
                    if (first.Point.Y > second.Point.Y) return 1;
                    if (first.Point.X < second.Point.X) return -1;
                    if (first.Point.X > second.Point.X) return 1;
                    return 0;
                }
                goodList.Sort(CompareHashPointFunc);
                int goodCount = goodList.Count;
                var goodPoints = new List<Point>(capacity: goodCount);
                var goodHashValues = new List<int>(capacity: goodCount);
                foreach (var goodHP in goodList)
                {
                    goodPoints.Add(goodHP.Point);
                    goodHashValues.Add(goodHP.HashValue);
                }
                return new HashPointList(goodPoints.AsReadOnly(), goodHashValues.AsReadOnly(), HashPointSortKey.HashValue);
            }
        }

        public class TwoRowSourceWrapper
        {
            public IBitmapRowSource<int> Source { get; }

            public Size Size { get; }

            private int _currRow;
            private int _prevRow;
            private int[] _currArray;
            private int[] _prevArray;
            private int _currOffset;
            private int _prevOffset;

            public int CurrRow => _currRow;

            public int[] CurrArray => _currArray;

            public int CurrOffset => _currOffset;

            public int PrevRow => _prevRow;

            public int[] PrevArray => _prevArray;

            public int PrevOffset => _prevOffset;

            public TwoRowSourceWrapper(IBitmapRowSource<int> source)
            {
                if (source is null)
                {
                    throw new ArgumentException(nameof(source));
                }
                Source = source;
                Size = source.Size;
                _prevRow = -1;
                _currRow = -1;
                if (!(source is IBitmapRowDirect<int>))
                {
                    int expectedCount = Size.Width;
                    _prevArray = new int[expectedCount];
                    _currArray = new int[expectedCount];
                }
            }

            public void LoadRow(int row)
            {
                if (row < 0 || row >= Size.Height)
                {
                    throw new ArgumentOutOfRangeException(nameof(row));
                }
                _SwapPrevCurr();
                _currRow = row;
                if (Source is IBitmapRowDirect<int> direct)
                {
                    var segment = direct.GetRowDirect(row);
                    int expectedCount = Size.Width;
                    if (segment.Array is null ||
                        segment.Count != expectedCount)
                    {
                        throw new Exception("Unexpected");
                    }
                    _currArray = segment.Array;
                    _currOffset = segment.Offset;
                }
                else
                {
                    Source.CopyRow(row, _currArray, _currOffset);
                }
            }

            private void _SwapPrevCurr()
            {
                _Swap(ref _prevRow, ref _currRow);
                _Swap(ref _prevArray, ref _currArray);
                _Swap(ref _prevOffset, ref _currOffset);
            }

            private void _Swap<T>(ref T prev, ref T curr)
            {
                T temp = prev;
                prev = curr;
                curr = temp;
            }
        }
    }
}
