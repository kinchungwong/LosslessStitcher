using System.IO;
using NUnit.Framework;
using LosslessStitcher.Imaging.IO;

namespace LosslessStitcher.Imaging.IO.Tests
{
    public class CodecSmokeTests
    {
        [Test]
        public void BitmapDecoder_SmokeTest_Png()
        {
            string filename = $"blank_1920x1080.png";
            var manager = new BitmapCodecManager();
            var factory = new BitmapFactory();
            using (var strm = File.OpenRead(filename))
            {
                var decoder = manager.GetDecoder(strm);
                IntBitmap bitmap = new IntBitmap(factory, 1920, 1080);
                decoder.Decode(bitmap);
            }
        }

        [Test]
        public void BitmapDecoder_SmokeTest_Png_LoadSave()
        {
            string filename = $"blank_1920x1080.png";
            string outputFilename = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".png");
            var manager = new BitmapCodecManager();
            var factory = new BitmapFactory();
            using (var strm = File.OpenRead(filename))
            {
                var decoder = manager.GetDecoder(strm);
                IntBitmap bitmap = new IntBitmap(factory, 1920, 1080);
                decoder.Decode(bitmap);
                var encoder = manager.GetEncoder(decoder.CodecIdentifier);
                using (var strm2 = File.OpenWrite(outputFilename))
                {
                    encoder.Encode(bitmap, strm2);
                }
            }
        }
    }
}
