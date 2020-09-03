using System;
using System.Drawing;
using System.IO;

namespace LosslessStitcher.Imaging.IO
{
    public class BitmapDecoder
        : IBitmapDecoder
    {
        public Stream Stream { get; }

        public IBitmapCodecIdentifier CodecIdentifier { get; private set; }

        public IBitmapInfo BitmapInfo { get; private set; }

        public BitmapDecoder(Stream stream)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            Stream = stream;
            _CtorPopulateBitmapInfo();
        }

        private void _CtorPopulateBitmapInfo()
        {
            // ====== TODO ======
            // Need to implement this without having to perform decoding of all bitmap pixels.
            // This method needs to be O(1) and must not read the entire file.
            // ======
            CodecIdentifier = BitmapCodecIdentifier.Default;
            BitmapInfo = new BitmapInfo(new Data.Size());
        }

        public void Decode<T>(IBitmapRowAccess<T> output)
            where T : struct
        {
            // ====== TODO ======
            // Currently, this bitmap decoder does not perform quick BitmapInfo.
            // Once quick BitmapInfo is implemented, the output argument's bitmap size
            // should be validated against the bitmap size obtained from the stream,
            // before the System.Drawing.Bitmap is created.
            // ======
            if (output is null)
            {
                throw new ArgumentNullException(nameof(output));
            }
            switch (output)
            {
                case IBitmapRowAccess<int> intOutput:
                    using (var bitmap = new Bitmap(Stream))
                    {
                        CopyToMethods.CopyTo(bitmap, intOutput);
                        return;
                    }
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
