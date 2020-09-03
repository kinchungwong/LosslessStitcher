using System.Drawing;
using System.Drawing.Imaging;

namespace LosslessStitcher.Imaging.IO
{
    public static class ConvertToMethods
    {
        public static Bitmap ToBitmap(this IntBitmap input)
        {
            var output = new Bitmap(input.Width, input.Height, PixelFormat.Format24bppRgb);
            CopyToMethods.CopyTo(input, output);
            return output;
        }

        public static IntBitmap ToIntBitmap(this Bitmap input)
        {
            return ToIntBitmap(input, new BitmapFactory());
        }

        public static IntBitmap ToIntBitmap(this Bitmap input, BitmapFactory factory)
        {
            var output = factory.CreateIntBitmap(input.Width, input.Height);
            CopyToMethods.CopyTo(input, output);
            return output;
        }
    }
}
