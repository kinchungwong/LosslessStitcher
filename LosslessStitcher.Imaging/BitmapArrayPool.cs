namespace LosslessStitcher.Imaging
{
    public class BitmapArrayPool<T>
    {
        public IArrayPool<T> ArrayPool { get; set; }

        public delegate T[] RentDelegate(int length);

        public delegate void ReturnDelegate(T[] array);

        public RentDelegate Rent 
        {
            get => _rent;
            set => _rent = value ?? DefaultRentDelegate;
        }

        public ReturnDelegate Return
        {
            get => _return;
            set => _return = value ?? DefaultReturnDelegate;
        }

        #region private
        private RentDelegate _rent;
        private ReturnDelegate _return;
        #endregion

        public T[] DefaultRentDelegate(int length)
        {
            if (!(ArrayPool is null))
            {
                return ArrayPool.Rent(length);
            }
            return new T[length];
        }

        public void DefaultReturnDelegate(T[] array)
        {
            if (!(ArrayPool is null))
            {
                ArrayPool.Return(array);
            }
        }

        public BitmapArrayPool()
        {
            Rent = DefaultRentDelegate;
            Return = DefaultReturnDelegate;
        }
    }
}
