using System;
using LosslessStitcher.Data;

namespace LosslessStitcher.Imaging
{
    /// <summary>
    /// <see cref="BitmapCopyWorker{T}"/> is a worker class for copying all pixels from one bitmap
    /// to another bitmap.
    /// </summary>
    /// 
    /// <typeparam name="T">
    /// The pixel type of the bitmaps to be copied.
    /// </typeparam>
    /// 
    /// <remarks>
    /// <para>
    /// This worker class uses eager interface downcasting to find the most optimal execution strategy
    /// for copying the pixels. If the source or dest instance implements a particular interface
    /// (accessible via downcasting), it is assumed to honor the contract of that interface, which 
    /// basically means more direct access and less overhead.
    /// </para>
    /// 
    /// <para>
    /// Acknowledgment.
    /// <br/>
    /// https://www.yegor256.com/2014/05/05/oop-alternative-to-utility-classes.html
    /// <br/>
    /// Prior to the refactoring, the old implementation consisted of four or five distinct static methods.
    /// After taking inspirations from the article and its code examples, it is refactored into the current
    /// form, which revealed additional optimization opportunities that were neglected in the old 
    /// implementation.
    /// </para>
    /// </remarks>
    /// 
    public class BitmapCopyWorker<T>
        where T : struct
    {
        public IBitmapRowSource<T> Source { get; }

        public IBitmapRowDirect<T> SourceDirect { get; }

        public IArrayBitmap<T> SourceArrayBitmap { get; }

        public IBitmapRowAccess<T> DestAccess { get; }

        public IBitmapRowDirect<T> DestDirect { get; }

        public IArrayBitmap<T> DestArrayBitmap { get; }

        public bool HasSourceArrayBitmap { get; }

        public bool HasSourceDirect { get; }

        public bool HasDestArrayBitmap { get; }

        public bool HasDestDirect { get; }

        public int Width { get; }

        public int Height { get; }

        public Size Size => new Size(Width, Height);

        private delegate void InternalInvokeDelegate();

        private InternalInvokeDelegate InternalInvokeFunc;

        public BitmapCopyWorker(IBitmapRowSource<T> source, IBitmapRowAccess<T> dest)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (dest is null)
            {
                throw new ArgumentNullException(nameof(dest));
            }
            if (source.Width != dest.Width ||
                source.Height != dest.Height)
            {
                throw new ArgumentException(message: "Bitmap size mismatch");
            }
            Width = source.Width;
            Height = source.Height;
            SourceArrayBitmap = source as IArrayBitmap<T>;
            SourceDirect = source as IBitmapRowDirect<T>;
            Source = source;
            DestArrayBitmap = dest as IArrayBitmap<T>;
            DestDirect = dest as IBitmapRowDirect<T>;
            DestAccess = dest;
            HasSourceArrayBitmap = !(SourceArrayBitmap is null);
            HasSourceDirect = !(SourceDirect is null);
            HasDestArrayBitmap = !(DestArrayBitmap is null);
            HasDestDirect = !(DestDirect is null);
            if (HasSourceArrayBitmap && HasDestArrayBitmap)
            {
                InternalInvokeFunc = _Invoke_SourceArrayBitmap_DestArrayBitmap;
            }
            else if (HasSourceDirect && HasDestDirect)
            {
                InternalInvokeFunc = _Invoke_SourceDirect_DestDirect;
            }
            else if (HasSourceDirect)
            {
                InternalInvokeFunc = _Invoke_SourceDirect_DestAccess;
            }
            else if (HasDestDirect)
            {
                InternalInvokeFunc = _Invoke_Source_DestDirect;
            }
            else
            {
                InternalInvokeFunc = _Invoke_Source_DestAccess;
            }
        }

        public void Invoke()
        {
            InternalInvokeFunc();
        }

        private void _Invoke_SourceArrayBitmap_DestArrayBitmap()
        {
            int count = checked(Width * Height);
            Array.Copy(SourceArrayBitmap.Data, DestArrayBitmap.Data, count);
        }

        private void _Invoke_SourceDirect_DestDirect()
        {
            for (int row = 0; row < Height; ++row)
            {
                var sourceSegment = SourceDirect.GetRowDirect(row);
                var destSegment = DestDirect.GetRowDirect(row);
                Array.Copy(sourceSegment.Array, sourceSegment.Offset, destSegment.Array, destSegment.Offset, Width);
            }
        }

        private void _Invoke_SourceDirect_DestAccess()
        {
            for (int row = 0; row < Height; ++row)
            {
                var sourceSegment = SourceDirect.GetRowDirect(row);
                DestAccess.WriteRow(row, sourceSegment.Array, sourceSegment.Offset);
            }
        }

        private void _Invoke_Source_DestDirect()
        {
            for (int row = 0; row < Height; ++row)
            {
                var destSegment = DestDirect.GetRowDirect(row);
                Source.CopyRow(row, destSegment.Array, destSegment.Offset);
            }
        }

        private void _Invoke_Source_DestAccess()
        {
            T[] buffer = new T[Width];
            for (int row = 0; row < Height; ++row)
            {
                Source.CopyRow(row, buffer, 0);
                DestAccess.WriteRow(row, buffer, 0);
            }
        }
    }
}
