namespace LosslessStitcher.Imaging.IO
{
    public class BitmapCodecIdentifier 
        : IBitmapCodecIdentifier
    {
        public static BitmapCodecIdentifier Default { get; } = new BitmapCodecIdentifier();
    }
}
