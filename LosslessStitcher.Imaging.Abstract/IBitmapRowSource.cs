namespace LosslessStitcher.Imaging
{
    public interface IBitmapRowSource<T>
        : IScalarBitmapInfo
        where T : struct
    {
        void CopyRow(int row, T[] dest, int destStart);
    }
}
