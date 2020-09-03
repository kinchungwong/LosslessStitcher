using LosslessStitcher.Data;

namespace LosslessStitcher.Imaging.IO
{
    public class BitmapInfo
        : IBitmapInfo
    {
        public int Width { get; }
        
        public int Height { get; }

        public Size Size => new Size(Width, Height);

        public BitmapInfo(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public BitmapInfo(Size size)
        {
            Width = size.Width;
            Height = size.Height;
        }
    }
}
