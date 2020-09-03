using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosslessStitcher.Imaging.IO.Internals
{
    /// <summary>
    /// Short-lived byte row buffers.
    /// </summary>
    /// 
    /// <remarks>
    /// Designed for use inside conversion wrapper from System.Drawing.Bitmap into IntBitmap.
    /// The lifetime of this buffer pool should be the same as the bitmap conversion wrapper.
    /// The array length is specific for the width of the bitmap, multiplied by the 
    /// bytes-per-pixel of that bitmap. For this reason, the global (project-wide) array pool 
    /// is not used, because the usage scenario only requires a single row buffer at any time
    /// and the opportunity for array reuse is very low.
    /// </remarks>
    /// 
    public class ByteRowBufferPool
    {
        private readonly object _lock;

        private readonly Stack<byte[]> _spares;

        public int ArrayLength { get; }

        public ByteRowBufferPool(int arrayLength)
        {
            if (arrayLength <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayLength));
            }
            _lock = new object();
            _spares = new Stack<byte[]>();
            ArrayLength = arrayLength;
        }

        public byte[] Rent()
        {
            lock (_lock)
            {
                if (_spares.Count > 0)
                {
                    return _spares.Pop();
                }
            }
            return new byte[ArrayLength];
        }

        public void Return(byte[] array)
        {
            if (array?.Length != ArrayLength)
            {
                return;
            }
            lock (_lock)
            {
                _spares.Push(array);
            }
        }
    }
}
