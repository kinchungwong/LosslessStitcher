using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace LosslessStitcher.Imaging.IO
{
    public static class BitmapCodecUtility
    {
        public static IntBitmap LoadAsIntBitmap(string filename)
        {
            return LoadAsIntBitmap(filename, new BitmapFactory());
        }

        public static IntBitmap LoadAsIntBitmap(string filename, BitmapFactory bitmapFactory)
        {
            using (var input = new Bitmap(filename))
            {
                return input.ToIntBitmap(bitmapFactory);
            }
        }

        public static IntBitmap LoadAsIntBitmap(Stream strm)
        {
            return LoadAsIntBitmap(strm, new BitmapFactory());
        }

        public static IntBitmap LoadAsIntBitmap(Stream strm, BitmapFactory bitmapFactory)
        {
            using (var input = new Bitmap(strm))
            {
                return input.ToIntBitmap(bitmapFactory);
            }
        }

        public static void SaveToFile(this IntBitmap bitmap, string filename)
        {
            using (var converted = bitmap.ToBitmap())
            {
                converted.Save(filename, ImageFormat.Png);
            }
        }

        public static MemoryStream SaveToMemoryStream(this IntBitmap bitmap)
        {
            MemoryStream strm = new MemoryStream();
            try
            {
                using (var converted = bitmap.ToBitmap())
                {
                    converted.Save(strm, ImageFormat.Png);
                }
            }
            catch
            {
                strm.Dispose();
                throw;
            }
            strm.Position = 0;
            return strm;
        }
    }
}
