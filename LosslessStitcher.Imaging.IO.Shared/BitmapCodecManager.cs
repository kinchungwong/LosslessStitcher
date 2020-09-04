using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;

namespace LosslessStitcher.Imaging.IO
{
    public class BitmapCodecManager
        : IBitmapCodecManager
    {
        public IEnumerable<IBitmapCodecIdentifier> Codecs
        {
            get;
        } = (new List<IBitmapCodecIdentifier>() { BitmapCodecIdentifier.Default }).AsReadOnly();

        public IBitmapCodecIdentifier Identify(Stream stream)
        {
            return BitmapCodecIdentifier.Default;
        }

        public IBitmapInfo GetBitmapInfo(Stream stream)
        {
            return GetDecoder(stream).BitmapInfo;
        }

        public IBitmapDecoder GetDecoder(Stream stream)
        {
            return new BitmapDecoder(stream);
        }

        public IBitmapEncoder GetEncoder(IBitmapCodecIdentifier codecId)
        {
            return new BitmapEncoder();
        }
    }
}
