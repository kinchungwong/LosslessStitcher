using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace LosslessStitcher.Imaging.IO.Internals
{
    public class BitmapIoWrapper
        : IDisposable
        , IBitmapRowAccess<int>
    {
        public Bitmap Bitmap { get; }

        public IBitmapRowAccess<int> Wrapper { get; private set; }

        public int Width => Wrapper.Width;

        public int Height => Wrapper.Height;

        public Data.Size Size => Wrapper.Size;

        public bool CanRead { get; }

        public bool CanWrite { get; }

        public bool ShouldDisposeBitmap { get; }

        public bool ShouldDisposeWrapper { get; }

        public BitmapIoWrapper(Bitmap bitmap, bool canRead, bool canWrite, bool shouldDisposeBitmap, bool shouldDisposeWrapper)
        {
            if (bitmap is null)
            {
                throw new ArgumentNullException(nameof(bitmap));
            }
            Bitmap = bitmap;
            ShouldDisposeBitmap = shouldDisposeBitmap;
            ShouldDisposeWrapper = shouldDisposeWrapper;
            CanRead = canRead;
            CanWrite = canWrite;
            var format = bitmap.PixelFormat;
            switch (format)
            {
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    {
                        var lockedBitmap = new LockedByteBitmap(bitmap, canRead, canWrite, shouldDisposeBitmap);
                        Wrapper = new Bgr8888Wrapper(lockedBitmap, true);
                        break;
                    }
                case PixelFormat.Format24bppRgb:
                    {
                        var lockedBitmap = new LockedByteBitmap(bitmap, canRead, canWrite, shouldDisposeBitmap);
                        Wrapper = new Bgr888Wrapper(lockedBitmap, true);
                        break;
                    }
                case PixelFormat.Format8bppIndexed:
                    {
                        throw new NotImplementedException();
                    }
                default:
                    throw new Exception($"Unsupported pixel format: 0x{((int)format):x8}");
            }
        }

        public void WriteRow(int row, int[] source, int sourceStart)
        {
            Wrapper.WriteRow(row, source, sourceStart);
        }

        public void CopyRow(int row, int[] dest, int destStart)
        {
            Wrapper.CopyRow(row, dest, destStart);
        }

        public void Dispose()
        {
            if (ShouldDisposeWrapper &&
                !(Wrapper is null))
            {
                (Wrapper as IDisposable)?.Dispose();
                Wrapper = null;
            }
        }
    }
}
