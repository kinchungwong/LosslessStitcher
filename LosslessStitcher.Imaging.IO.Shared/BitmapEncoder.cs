using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace LosslessStitcher.Imaging.IO
{
    public class BitmapEncoder
        : IBitmapEncoder
    {
        public IBitmapCodecIdentifier CodecIdentifier => BitmapCodecIdentifier.Default;

        public ImageFormat ImageFormat { get; set; }

        public BitmapEncoder()
        {
            ImageFormat = ImageFormat.Png;
        }

        public void Encode<T>(IBitmapRowSource<T> input, Stream output)
            where T : struct
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            if (output is null)
            {
                throw new ArgumentNullException(nameof(output));
            }
            int width = input.Width;
            int height = input.Height;
            switch (input)
            {
                case IBitmapRowSource<int> intBitmap:
                    using (var bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb))
                    {
                        CopyToMethods.CopyTo(intBitmap, bitmap);
                        bitmap.Save(output, ImageFormat);
                        return;
                    }
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
