using System;
using System.Collections.Generic;
using System.Text;

namespace LosslessStitcher.Imaging.Hash2D
{
    public interface IHashValueRangeFilter
    {
        int FilterRangeMin { get; }

        int FilterRangeMax { get; }
    }
}
