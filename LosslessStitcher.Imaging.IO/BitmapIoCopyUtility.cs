using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace LosslessStitcher.Imaging.IO
{
    using Internals;

    public static class BitmapIoCopyUtility
    {
        public static void CopyTo(Bitmap input, IntBitmap output)
        {
            #region validation
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            if (output is null)
            {
                throw new ArgumentNullException(nameof(output));
            }
            if (input.Width != output.Width ||
                input.Height != output.Height)
            {
                throw new ArgumentException(message: "Bitmap size mismatch");
            }
            #endregion
            var format = input.PixelFormat;
            switch (format)
            {
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    {
                        using (var lockedBitmap = new LockedByteBitmap(input, true, false, shouldDispose: false))
                        {
                            var converter = new Bgr8888Wrapper(lockedBitmap, shouldDispose: false);
                            _CopyRows(converter, output);
                        }
                    }
                    return;
                case PixelFormat.Format24bppRgb:
                    {
                        using (var lockedBitmap = new LockedByteBitmap(input, true, false, shouldDispose: false))
                        {
                            var converter = new Bgr888Wrapper(lockedBitmap, shouldDispose: false);
                            _CopyRows(converter, output);
                        }
                    }
                    return;
                case PixelFormat.Format8bppIndexed:
                    {
                        throw new NotImplementedException();
                    }
                default:
                    throw new Exception($"Unsupported pixel format: 0x{((int)format):x8}");
            }
        }

        public static void CopyTo(IntBitmap input, Bitmap output)
        {
            #region validation
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            if (output is null)
            {
                throw new ArgumentNullException(nameof(output));
            }
            if (input.Width != output.Width ||
                input.Height != output.Height)
            {
                throw new ArgumentException(message: "Bitmap size mismatch");
            }
            #endregion
            var format = output.PixelFormat;
            switch (format)
            {
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    {
                        using (var lockedBitmap = new LockedByteBitmap(output, false, true, shouldDispose: false))
                        {
                            var converter = new Bgr8888Wrapper(lockedBitmap, shouldDispose: false);
                            _CopyRows(input, converter);
                        }
                    }
                    return;
                case PixelFormat.Format24bppRgb:
                    {
                        using (var lockedBitmap = new LockedByteBitmap(output, false, true, shouldDispose: false))
                        {
                            var converter = new Bgr888Wrapper(lockedBitmap, shouldDispose: false);
                            _CopyRows(input, converter);
                        }
                    }
                    return;
                case PixelFormat.Format8bppIndexed:
                    {
                        throw new NotImplementedException();
                    }
                default:
                    throw new Exception($"Unsupported pixel format: 0x{((int)format):x8}");
            }
        }

        private static void _CopyRows(IBitmapRowSource<int> source, IBitmapRowAccess<int> dest)
        {
            new BitmapCopyWorker<int>(source, dest).Invoke();
        }
    }
}
