using LosslessStitcher.Data;

namespace LosslessStitcher.Imaging
{
    public interface IArrayBitmap<T>
        : IBitmapRowDirect<T>
        where T : struct
    {
        T[] Data { get; }

        T this[Point p] { get; set; }

        T this[int x, int y] { get; set; }
    }
}
