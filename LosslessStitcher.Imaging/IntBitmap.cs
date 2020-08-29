using System;
using LosslessStitcher.Data;

namespace LosslessStitcher.Imaging
{
    public class IntBitmap
        : IArrayBitmap<int>
    {
        public BitmapFactory Factory { get; }

        public int Width { get; }

        public int Height { get; }

        public Size Size => new Size(Width, Height);

        public int[] Data { get; private set; }

        public int this[Point p]
        {
            get => Data[_GetPixelOffset(p.X, p.Y)];
            set => Data[_GetPixelOffset(p.X, p.Y)] = value;
        }

        public int this[int x, int y]
        {
            get => Data[_GetPixelOffset(x, y)];
            set => Data[_GetPixelOffset(x, y)] = value;
        }

        private IntBitmap(BitmapFactory factory)
        {
            Factory = factory;
        }

        public IntBitmap(BitmapFactory factory, int width, int height)
            : this(factory)
        {
            _CtorValidateWH(width, height);
            Width = width;
            Height = height;
            Data = factory.IntArrayPool.Rent(checked(width * height));
        }

        public IntBitmap(BitmapFactory factory, Size size)
            : this(factory, size.Width, size.Height)
        {
        }

        public void Dispose()
        {
            var data = Data;
            Data = null;
            if (!(data is null))
            {
                Factory.IntArrayPool.Return(data);
            }
        }

        private int _GetPixelOffset(int x, int y)
        {
            _ValidateXY(x, y);
            return y * Width + x;
        }

        private void _ValidateXY(int x, int y)
        {
            if (x < 0 || x >= Width)
            {
                throw new ArgumentOutOfRangeException(nameof(x));
            }
            if (y < 0 || y >= Height)
            {
                throw new ArgumentOutOfRangeException(nameof(y));
            }
        }

        private static void _CtorValidateWH(int width, int height)
        {
            if (width <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width));
            }
            if (height <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(height));
            }
        }
    }
}
