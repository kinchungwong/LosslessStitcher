using System.Drawing;
using System.Drawing.Imaging;

namespace LosslessStitcher.Imaging.IO
{
    public static class BitmapIoConvertUtility
    {
        public static Bitmap ToBitmap(this IntBitmap input)
        {
            var output = new Bitmap(input.Width, input.Height, PixelFormat.Format24bppRgb);
            BitmapIoCopyUtility.CopyTo(input, output);
            return output;
        }

        public static IntBitmap ToIntBitmap(this Bitmap input)
        {
            return ToIntBitmap(new BitmapFactory(), input);
        }

        public static IntBitmap ToIntBitmap(BitmapFactory factory, Bitmap input)
        {
            var output = factory.CreateIntBitmap(input.Width, input.Height);
            BitmapIoCopyUtility.CopyTo(input, output);
            return output;
        }
    }
}
