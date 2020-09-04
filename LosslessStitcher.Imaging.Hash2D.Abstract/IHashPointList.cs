using System;
using System.Collections.Generic;

namespace LosslessStitcher.Imaging.Hash2D
{
    using LosslessStitcher.Data;

    public interface IHashPointList
        : IReadOnlyList<(Point Point, int HashValue)>
    {
        IReadOnlyList<Point> Points { get; }

        IReadOnlyList<int> HashValues { get; }
    }
}
