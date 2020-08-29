namespace LosslessStitcher.Imaging
{
    public interface IBitmapRowSource<T>
        : IBitmapInfo
        where T : struct
    {
        void CopyRow(int row, T[] dest, int destStart);
    }
}
