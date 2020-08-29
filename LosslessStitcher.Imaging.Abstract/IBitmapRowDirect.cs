using System;

namespace LosslessStitcher.Imaging
{
    public interface IBitmapRowDirect<T>
        : IBitmapRowAccess<T>
        where T : struct
    {
        ArraySegment<T> GetRowDirect(int row);
    }
}
