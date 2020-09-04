using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace LosslessStitcher.Imaging.Hash2D
{
    public interface IUniqueHashFilter
    {
        IHashPointList Extract(IBitmapRowSource<int> hash2dBitmap);
    }
}
