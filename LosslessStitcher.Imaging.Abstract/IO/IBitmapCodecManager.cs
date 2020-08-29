using System.IO;

namespace LosslessStitcher.Imaging.IO
{
    public interface IBitmapCodecManager
    {
        IBitmapCodecIdentifier Identify(Stream stream);

        IBitmapInfo GetBitmapInfo(Stream stream);

        IBitmapDecoder GetDecoder(Stream stream);

        IBitmapEncoder GetEncoder(IBitmapCodecIdentifier codecId);
    }
}
