using Autofac;
using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace LosslessStitcher.Imaging.Hash2D.Tests
{
    using LosslessStitcher.Imaging.IO;

    public class SimpleProcessor_SmokeTest
    {
        public TestContainer TestContainer = new TestContainer();
        public IContainer Container => TestContainer.Container;

        [Test]
        [TestCase(typeof(TestBitmap_Blank))]
        [TestCase(typeof(TestBitmap_TakeX))]
        [TestCase(typeof(TestBitmap_TakeY))]
        [TestCase(typeof(TestBitmap_SumXY))]
        [TestCase(typeof(TestBitmap_ProdXY))]
        [TestCase(typeof(TestBitmap_HashXY))]
        public void SmokeTest(Type testBitmapType)
        {
            int width = 16;
            int height = 16;
            var bitmapFactory = Container.Resolve<IBitmapFactory>();
            IArrayBitmap<int> input = (Activator.CreateInstance(testBitmapType) as TestBitmapFunc).Generate(bitmapFactory, width, height);
            IArrayBitmap<int> output = bitmapFactory.Create<int>(width, height);
            IHash2DProcessor proc = Container.Resolve<IHash2DProcessor>();
            proc.AddStage(Direction.Horz, 3, 1);
            proc.AddStage(Direction.Vert, 3, 1);
            proc.Process(input, output);
            string ofn1 = testBitmapType.Name;
            string ofn2 = Path.GetRandomFileName();
            string ofn3 = $"{nameof(SimpleProcessor_SmokeTest)}_{nameof(SmokeTest)}_{ofn1}_{ofn2}.png";
            string outputFilename = Path.Combine(Path.GetTempPath(), ofn3);
            _SaveBitmap(output, outputFilename);
        }

        private void _SaveBitmap(IArrayBitmap<int> bitmap, string outputFilename)
        {
            var manager = Container.Resolve<IBitmapCodecManager>();
            var codec = new List<IBitmapCodecIdentifier>(manager.Codecs)[0];
            var encoder = manager.GetEncoder(codec);
            using (var strm = File.OpenWrite(outputFilename))
            {
                encoder.Encode(bitmap, strm);
            }
        }
    }
}
