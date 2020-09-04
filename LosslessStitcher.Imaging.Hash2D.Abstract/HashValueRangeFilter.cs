using System;
using System.Collections.Generic;
using System.Text;

namespace LosslessStitcher.Imaging.Hash2D
{
    public class HashValueRangeFilter
        : IHashValueRangeFilter
    {
        public double? FilterFrac { get; }

        public int FilterRangeMin { get; }

        public int FilterRangeMax { get; }

        public HashValueRangeFilter(double filterFrac)
        {
            if (!(filterFrac >= 0.0 && filterFrac <= 1.0))
            {
                throw new ArgumentOutOfRangeException(nameof(filterFrac));
            }
            FilterFrac = filterFrac;
            FilterRangeMin = (int)Math.Round(filterFrac * int.MinValue);
            FilterRangeMax = (int)Math.Round(filterFrac * int.MaxValue);
        }

        public HashValueRangeFilter(IHashValueRangeFilter hashValueRangeFilter)
        {
            if (hashValueRangeFilter is HashValueRangeFilter other)
            {
                FilterFrac = other.FilterFrac;
            }
            else
            {
                FilterFrac = null;
            }
            FilterRangeMin = hashValueRangeFilter.FilterRangeMin;
            FilterRangeMax = hashValueRangeFilter.FilterRangeMax;
        }
    }
}
