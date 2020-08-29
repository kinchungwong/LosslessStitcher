using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace LosslessStitcher.Imaging.IO.Internals
{
    public class LockedByteBitmap 
        : IDisposable
        , IBitmapRowAccess<byte>
    {
        private readonly object _lock = new object();

        public Bitmap Bitmap { get; private set; }

        public BitmapData BitmapData { get; private set; }

        public PixelFormat PixelFormat { get; }

        public bool CanRead { get; }

        public bool CanWrite { get; }

        public bool ShouldDisposeBitmap { get; set; }

        private ImageLockMode LockFlags { get; }

        public int Width { get; }

        public int Height { get; }

        Data.Size IBitmapInfo.Size => new Data.Size(Width, Height);

        public int BytesPerPixel { get; }

        public int ArrayElementsPerRow { get; }

        private IntPtr Scan0;

        private int Stride;

        public LockedByteBitmap(Bitmap bitmap, bool canRead, bool canWrite, bool shouldDisposeBitmap)
        {
            Bitmap = bitmap;
            PixelFormat = bitmap.PixelFormat;
            Width = bitmap.Width;
            Height = bitmap.Height;
            CanRead = canRead;
            CanWrite = canWrite;
            ShouldDisposeBitmap = shouldDisposeBitmap;
            LockFlags = _GetFlags(canRead, canWrite);
            BytesPerPixel = _GetBytesPerPixel(PixelFormat);
            ArrayElementsPerRow = Width * BytesPerPixel;
            var rect = new Rectangle(0, 0, Width, Height);
            BitmapData = Bitmap.LockBits(rect, LockFlags, PixelFormat);
            Scan0 = BitmapData.Scan0;
            Stride = BitmapData.Stride;
        }

        public void Dispose()
        {
            lock (_lock)
            {
                if (!(Bitmap is null) &&
                    !(BitmapData is null))
                {
                    Bitmap.UnlockBits(BitmapData);
                }
                if (ShouldDisposeBitmap &&
                    !(Bitmap is null))
                {
                    Bitmap.Dispose();
                }
                Bitmap = null;
                BitmapData = null;
                Scan0 = IntPtr.Zero;
                Stride = 0;
            }
        }

        public void CopyRow(int row, byte[] dest, int destStart)
        {
            if (!CanRead)
            {
                throw new InvalidOperationException();
            }
            if (destStart + ArrayElementsPerRow > dest.Length)
            {
                throw new InvalidOperationException();
            }
            Marshal.Copy(Scan0 + Stride * row, dest, destStart, ArrayElementsPerRow);
        }

        public void WriteRow(int row, byte[] source, int sourceStart)
        {
            if (!CanWrite)
            {
                throw new InvalidOperationException();
            }
            if (sourceStart + ArrayElementsPerRow > source.Length)
            {
                throw new InvalidOperationException();
            }
            Marshal.Copy(source, sourceStart, Scan0 + Stride * row, ArrayElementsPerRow);
        }

        private static int _GetBytesPerPixel(PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    return 4;
                case PixelFormat.Format24bppRgb:
                    return 3;
                case PixelFormat.Format8bppIndexed:
                    return 1;
                default:
                    throw new NotImplementedException();
            }
        }

        private static ImageLockMode _GetFlags(bool canRead, bool canWrite)
        {
            int code = (canRead ? 1 : 0) + (canWrite ? 2 : 0);
            switch (code)
            {
                case 1:
                    return ImageLockMode.ReadOnly;
                case 2:
                    return ImageLockMode.WriteOnly;
                case 3:
                    return ImageLockMode.ReadWrite;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
