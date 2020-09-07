using System;
using System.Collections.Generic;
using CollectionsUtility.Mocks;

namespace LosslessStitcher.Tracking.T3
{
    using LosslessStitcher.Data;
    using LosslessStitcher.Imaging.Hash2D;

    public class T3TrajectoryLabeler
    {
        public T3ImageKeys ImageKeys { get; }

        public Func<int, IHashPointList> ImageHashPointSource { get; }

        public IReadOnlyList<int> HashValues { get; }

        public IReadOnlyList<(Point Point0, Point Point1, Point Point2)> Points { get; }

        public T3TrajectoryLabeler(IEnumerable<int> imageKeys, Func<int, IHashPointList> imageHashPointSource)
        {
            ImageKeys = new T3ImageKeys(imageKeys);
            ImageHashPointSource = imageHashPointSource;
            var hp0 = ImageHashPointSource(ImageKeys.ItemAt(0));
            var hp1 = ImageHashPointSource(ImageKeys.ItemAt(1));
            var hp2 = ImageHashPointSource(ImageKeys.ItemAt(2));
            if (hp0.SortKey != HashPointSortKey.HashValue ||
                hp1.SortKey != HashPointSortKey.HashValue ||
                hp2.SortKey != HashPointSortKey.HashValue)
            {
                throw new InvalidOperationException();
            }
            var hp012 = new SortedIntListIntersect(hp0.HashValues, hp1.HashValues, hp2.HashValues);
            HashValues = new List<int>(hp012).AsReadOnly();
            var pts012 = new List<(Point Point0, Point Point1, Point Point2)>();
            int index0 = 0;
            int index1 = 0;
            int index2 = 0;
            foreach (int hashValue in HashValues)
            {
                while (index0 < hp0.Count &&
                    hp0.HashValues[index0] < hashValue)
                {
                    ++index0;
                }
                while (index1 < hp1.Count &&
                    hp1.HashValues[index1] < hashValue)
                {
                    ++index1;
                }
                while (index2 < hp2.Count &&
                    hp2.HashValues[index2] < hashValue)
                {
                    ++index2;
                }
                pts012.Add((hp0.Points[index0], hp1.Points[index1], hp2.Points[index2]));
            }
        }
    }
}
