using System.IO;

namespace LosslessStitcher.Imaging.IO
{
    public interface IBitmapDecoder
    {
        Stream Stream { get; }

        IBitmapCodecIdentifier CodecIdentifier { get; }

        IBitmapInfo BitmapInfo { get; }

        void Decode<T>(IBitmapRowAccess<T> output)
            where T : struct;
    }
}
