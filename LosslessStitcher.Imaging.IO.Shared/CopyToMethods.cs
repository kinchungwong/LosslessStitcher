using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace LosslessStitcher.Imaging.IO
{
    using Internals;

    public static class CopyToMethods
    {
        public static void CopyTo(this Bitmap input, IBitmapRowAccess<int> output)
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
            using (var inputWrapper = new BitmapIoWrapper(input, true, false, false, true))
            {
                new BitmapCopyWorker<int>(inputWrapper, output).Invoke();
            }
        }

        public static void CopyTo(this IBitmapRowSource<int> input, Bitmap output)
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
            using (var outputWrapper = new BitmapIoWrapper(output, false, true, false, true))
            {
                new BitmapCopyWorker<int>(input, outputWrapper).Invoke();
            }
        }
    }
}
