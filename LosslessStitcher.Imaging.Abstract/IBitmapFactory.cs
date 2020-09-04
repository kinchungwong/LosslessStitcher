namespace LosslessStitcher.Imaging
{
    public interface IBitmapFactory
    {
        IArrayBitmap<T> Create<T>(int width, int height)
            where T : struct;
    }
}
