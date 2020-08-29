using LosslessStitcher.Data;

namespace LosslessStitcher.Imaging
{
    public interface IBitmapInfo
    {
        int Width { get; }
        int Height { get; }
        Size Size { get; }
    }
}
