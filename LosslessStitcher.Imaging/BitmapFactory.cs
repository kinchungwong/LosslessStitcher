using LosslessStitcher.Data;

namespace LosslessStitcher.Imaging
{
    public class BitmapFactory
    {
        public BitmapArrayPool<int> IntArrayPool { get; set; } = new BitmapArrayPool<int>();

        public IntBitmap CreateIntBitmap(int width, int height)
        {
            return new IntBitmap(this, width, height);
        }

        public IntBitmap CreateIntBitmap(Size size)
        {
            return new IntBitmap(this, size);
        }
    }
}
