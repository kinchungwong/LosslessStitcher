using LosslessStitcher.Data;
using System;

namespace LosslessStitcher.Imaging
{
    public class BitmapFactory
    {
        public BitmapArrayPool<int> IntArrayPool { get; set; } = new BitmapArrayPool<int>();

        public IntBitmap CreateIntBitmap(int width, int height)
        {
            return Create<int>(width, height) as IntBitmap;
        }

        public IntBitmap CreateIntBitmap(Size size)
        {
            return Create<int>(size.Width, size.Height) as IntBitmap;
        }

        public IArrayBitmap<T> Create<T>(int width, int height)
            where T : struct
        {
            switch (default(T))
            {
                case int _:
                    return new IntBitmap(this, width, height) as IArrayBitmap<T>;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
