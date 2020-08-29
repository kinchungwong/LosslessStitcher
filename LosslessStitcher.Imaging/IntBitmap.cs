using System;
using LosslessStitcher.Data;

namespace LosslessStitcher.Imaging
{
    public class IntBitmap
        : IArrayBitmap<int>
        , IBitmapRowDirect<int>
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
            _CtorValidateFactory(factory);
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

        public void CopyRow(int row, int[] dest, int destStart)
        {
            _ValidateY(row);
            _ValidateRowBuffer(dest, destStart);
            int[] source = Data;
            int sourceStart = row * Width;
            Array.Copy(source, sourceStart, dest, destStart, Width);
        }

        public void WriteRow(int row, int[] source, int sourceStart)
        {
            _ValidateY(row);
            _ValidateRowBuffer(source, sourceStart);
            int[] dest = Data;
            int destStart = row * Width;
            Array.Copy(source, sourceStart, dest, destStart, Width);
        }

        public ArraySegment<int> GetRowDirect(int row) 
        {
            _ValidateY(row);
            int[] data = Data;
            int start = row * Width;
            int count = Width;
            return new ArraySegment<int>(data, start, count);
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

        private void _ValidateY(int y)
        {
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

        private void _ValidateRowBuffer(int[] rowData, int rowDataStart)
        {
            if (rowData is null)
            {
                throw new ArgumentNullException(nameof(rowData));
            }
            if (rowDataStart < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(rowDataStart));
            }
            int rowDataEnd = checked(rowDataStart + Width);
            int rowDataLength = rowData.Length;
            if (rowDataEnd > rowDataLength)
            {
                throw new IndexOutOfRangeException();
            }
        }

        private static void _CtorValidateFactory(BitmapFactory factory)
        {
            if (factory is null)
            {
                throw new ArgumentNullException(nameof(factory));
            }
        }
    }
}
