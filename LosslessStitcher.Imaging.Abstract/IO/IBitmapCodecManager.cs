using System.Collections.Generic;
using System.IO;

namespace LosslessStitcher.Imaging.IO
{
    public interface IBitmapCodecManager
    {
        IEnumerable<IBitmapCodecIdentifier> Codecs { get; }

        IBitmapCodecIdentifier Identify(Stream stream);

        IBitmapInfo GetBitmapInfo(Stream stream);

        IBitmapDecoder GetDecoder(Stream stream);

        IBitmapEncoder GetEncoder(IBitmapCodecIdentifier codecId);
    }
}
