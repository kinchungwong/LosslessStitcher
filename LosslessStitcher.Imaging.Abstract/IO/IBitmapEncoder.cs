﻿using System.IO;

namespace LosslessStitcher.Imaging.IO
{
    public interface IBitmapEncoder
    {
        IBitmapCodecIdentifier CodecIdentifier { get; }

        void Encode<T>(IBitmapRowSource<T> input, Stream output)
            where T : struct;
    }
}
