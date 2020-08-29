using System;
using LosslessStitcher.Data;

namespace LosslessStitcher.Imaging.IO.Internals
{
    /// <summary>
    /// Wraps an <see cref="System.Drawing.Bitmap"/> with 24-bit BGR pixel format.
    /// 
    /// <para>
    /// See also <see cref="Bgr8888Wrapper"/>.
    /// </para>
    /// </summary>
    /// 
    public class Bgr888Wrapper
        : IDisposable
        , IBitmapRowAccess<int>
    {
        private const int _bytesPerPixel = 3;

        public LockedByteBitmap Target { get; }

        public bool ShouldDispose { get; set; }

        public int Width { get; }

        public int Height { get; }

        public Size Size => new Size(Width, Height);

        public int BytesPerPixel => _bytesPerPixel;

        public int ArrayElementsPerRow { get; }

        private ByteRowBufferPool ByteRowBufferPool;

        public Bgr888Wrapper(LockedByteBitmap target, bool shouldDispose)
        {
            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }
            if (target.BytesPerPixel != _bytesPerPixel)
            {
                throw new InvalidOperationException("Wrong BytesPerPixel.");
            }
            Target = target;
            ShouldDispose = shouldDispose;
            ArrayElementsPerRow = target.ArrayElementsPerRow;
            ByteRowBufferPool = new ByteRowBufferPool(ArrayElementsPerRow);
            Width = target.Width;
            Height = target.Height;
        }

        public void CopyRow(int row, int[] dest, int destStart)
        {
            byte[] buffer = ByteRowBufferPool.Rent();
            if (Width <= 0 ||
                buffer is null ||
                buffer.Length < checked(Width * _bytesPerPixel))
            {
                // Catches an impossible event for the purpose of hinting compiler array bounds check elimination.
                return;
            }
            Target.CopyRow(row, buffer, 0);
            for (int k = 0; k < Width; ++k)
            {
                byte blue = buffer[k * _bytesPerPixel];
                byte green = buffer[k * _bytesPerPixel + 1];
                byte red = buffer[k * _bytesPerPixel + 2];
                dest[destStart + k] = _BgrToInt(red, green, blue);
            }
            ByteRowBufferPool.Return(buffer);
        }

        public void WriteRow(int row, int[] source, int sourceStart)
        {
            byte[] buffer = ByteRowBufferPool.Rent();
            if (Width <= 0 ||
                buffer is null ||
                buffer.Length < checked(Width * _bytesPerPixel))
            {
                // Catches an impossible event for the purpose of hinting compiler array bounds check elimination.
                return;
            }
            for (int k = 0; k < Width; ++k)
            {
                int intBgr32 = source[sourceStart + k];
                _IntToBgr(intBgr32, out byte red, out byte green, out byte blue);
                buffer[k * _bytesPerPixel] = blue;
                buffer[k * _bytesPerPixel + 1] = green;
                buffer[k * _bytesPerPixel + 2] = red;
            }
            Target.WriteRow(row, buffer, 0);
            ByteRowBufferPool.Return(buffer);
        }

        public void Dispose()
        {
            if (ShouldDispose &&
                !(Target is null))
            {
                Target.Dispose();
            }
        }

        private static int _BgrToInt(byte red, byte green, byte blue)
        { 
            unchecked
            {
                return (int)(blue | ((uint)green << 8) | ((uint)red << 16));
            }
        }

        private static void _IntToBgr(int value, out byte red, out byte green, out byte blue)
        {
            unchecked 
            {
                uint bgr32 = (uint)value;
                blue = (byte)(bgr32 & 0xFFu);
                green = (byte)((bgr32 >> 8) & 0xFFu);
                red = (byte)((bgr32 >> 16) & 0xFFu);
            }
        }
    }
}
