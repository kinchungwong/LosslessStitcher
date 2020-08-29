using System;

namespace LosslessStitcher
{
    public interface IArrayPool<T>
    {
        T[] Rent(int length);

        T[] Rent(int minLength, int maxLength);

        void Return(T[] array);
    }
}
