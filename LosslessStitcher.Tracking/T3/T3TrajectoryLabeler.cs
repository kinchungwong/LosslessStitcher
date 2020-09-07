using System;
using System.Collections.Generic;
using CollectionsUtility;
using CollectionsUtility.Mocks;

namespace LosslessStitcher.Tracking.T3
{
    using CollectionsUtility.Specialized;
    using LosslessStitcher.Data;
    using LosslessStitcher.Imaging.Hash2D;

    public class T3TrajectoryLabeler
    {
        public T3ImageKeys ImageKeys { get; }

        public int ImageKey0 => ImageKeys.ItemAt(0);

        public int ImageKey1 => ImageKeys.ItemAt(1);

        public int ImageKey2 => ImageKeys.ItemAt(2);

        public IReadOnlyList<int> HashValues { get; }

        public IReadOnlyList<(Point Point0, Point Point1, Point Point2)> Points { get; }

        public IReadOnlyUniqueList<(Movement, Movement)> Movements { get; }

        public IReadOnlyList<int> Labels { get; }

        public IReadOnlyDictionary<int, int> LabelPointCounts { get; }

        public T3TrajectoryLabeler(T3ImageKeys imageKeys, Func<int, IHashPointList> imageHashPointSource)
        {
            if (imageKeys is null)
            {
                throw new ArgumentNullException(nameof(imageKeys));
            }
            if (imageHashPointSource is null)
            {
                throw new ArgumentNullException(nameof(imageHashPointSource));
            }
            ImageKeys = imageKeys;
            var hps0 = imageHashPointSource(ImageKey0);
            var hps1 = imageHashPointSource(ImageKey1);
            var hps2 = imageHashPointSource(ImageKey2);
            if (hps0.SortKey != HashPointSortKey.HashValue ||
                hps1.SortKey != HashPointSortKey.HashValue ||
                hps2.SortKey != HashPointSortKey.HashValue)
            {
                throw new InvalidOperationException(
                    message: "This class requires hash points to be sorted by hash value.");
            }
            var hvs0 = hps0.HashValues;
            var hvs1 = hps1.HashValues;
            var hvs2 = hps2.HashValues;
            var pts0 = hps0.Points;
            var pts1 = hps1.Points;
            var pts2 = hps2.Points;
            var hvs012i = new SortedIntListIntersect(hvs0, hvs1, hvs2);
            var pts012i = new List<(Point Point0, Point Point1, Point Point2)>();
            int index0 = 0;
            int index1 = 0;
            int index2 = 0;
            var movements = new UniqueList<(Movement, Movement)>();
            var labels = new List<int>();
            var labelHist = HistogramFactory<int, int>.Create();
            foreach (int hashValue in hvs012i)
            {
                _StcSortedFindNext(hvs0, hashValue, ref index0);
                _StcSortedFindNext(hvs1, hashValue, ref index1);
                _StcSortedFindNext(hvs2, hashValue, ref index2);
                var pt0 = pts0[index0];
                var pt1 = pts1[index1];
                var pt2 = pts2[index2];
                pts012i.Add((pt0, pt1, pt2));
                var m01 = pt1 - pt0;
                var m12 = pt2 - pt1;
                int label = movements.Add((m01, m12));
                labels.Add(label);
                labelHist.Add(label);
            }
            HashValues = new List<int>(hvs012i).AsReadOnly();
            Points = pts012i.AsReadOnly();
            Movements = movements.AsReadOnly();
            Labels = labels.AsReadOnly();
            LabelPointCounts = labelHist.ToDictionary().AsReadOnly();
        }

        private static void _StcSortedFindNext(IReadOnlyList<int> list, int valueToFind, ref int index)
        {
            int count = list.Count;
            while (index < count &&
                list[index] < valueToFind)
            {
                ++index;
            }
        }
    }
}
