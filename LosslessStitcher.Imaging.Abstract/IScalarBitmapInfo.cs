using LosslessStitcher.Data;

namespace LosslessStitcher.Imaging
{
    public interface IScalarBitmapInfo
    {
        int Width { get; }
        int Height { get; }
        Size Size { get; }
    }
}
