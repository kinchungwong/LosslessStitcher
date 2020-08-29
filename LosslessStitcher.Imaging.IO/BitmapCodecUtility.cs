using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace LosslessStitcher.Imaging.IO
{
    public static class BitmapCodecUtility
    {
        public static IntBitmap LoadAsIntBitmap(string filename)
        {
            using (var input = new Bitmap(filename))
            {
                return input.ToIntBitmap();
            }
        }

        public static IntBitmap LoadAsIntBitmap(Stream strm)
        {
            using (var input = new Bitmap(strm))
            {
                return input.ToIntBitmap();
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
