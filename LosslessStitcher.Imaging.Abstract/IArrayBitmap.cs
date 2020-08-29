﻿using LosslessStitcher.Data;

namespace LosslessStitcher.Imaging
{
    public interface IArrayBitmap<T>
        : IBitmapInfo
        where T : struct
    {
        T[] Data { get; }

        T this[Point p] { get; set; }

        T this[int x, int y] { get; set; }
    }
}
