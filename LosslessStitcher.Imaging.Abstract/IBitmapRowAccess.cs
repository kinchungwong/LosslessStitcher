namespace LosslessStitcher.Imaging
{
    public interface IBitmapRowAccess<T>
        : IBitmapRowSource<T>
        where T : struct
    {
        void WriteRow(int row, T[] source, int sourceStart);
    }
}
