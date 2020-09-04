using Autofac;
using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace LosslessStitcher.Imaging.Hash2D.Tests
{
    using HashCodeUtility;
    using LosslessStitcher.Data;
    using LosslessStitcher.Functional;
    using LosslessStitcher.Imaging.Hash2D.Simple;
    using LosslessStitcher.Imaging.IO;

    public class SimpleFilter_SmokeTest
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
            IArrayBitmap<int> hashed = _ProcessHash(input);
            IArrayBitmap<int> filtered = _FilterHash(hashed);
            string ofn1 = testBitmapType.Name;
            string ofn2 = Path.GetRandomFileName();
            string ofn3 = $"{nameof(SimpleFilter_SmokeTest)}_{nameof(SmokeTest)}_{ofn1}_{ofn2}.png";
            string outputFilename = Path.Combine(Path.GetTempPath(), ofn3);
            _SaveBitmap(filtered, outputFilename);
        }

        private IArrayBitmap<int> _ProcessHash(IArrayBitmap<int> input)
        {
            Size size = input.Size;
            var bitmapFactory = Container.Resolve<IBitmapFactory>();
            IArrayBitmap<int> output = bitmapFactory.Create<int>(size.Width, size.Height);
            IHash2DProcessor proc = Container.Resolve<IHash2DProcessor>();
            proc.AddStage(Direction.Horz, 3, 1);
            proc.AddStage(Direction.Vert, 3, 1);
            proc.Process(input, output);
            return output;
        }

        private IArrayBitmap<int> _FilterHash(IArrayBitmap<int> hashed)
        {
            Size size = hashed.Size;
            var filter = Container.Resolve<IUniqueHashFilter>(new NamedParameter("hashValueRangeFilter", new HashValueRangeFilter(0.1)));
            var filtered = filter.Extract(hashed);
            var bitmapFactory = Container.Resolve<IBitmapFactory>();
            IArrayBitmap<int> output = bitmapFactory.Create<int>(size.Width, size.Height);
            foreach (var hashPoint in filtered)
            {
                output[hashPoint.Point] = hashPoint.HashValue;
            }
            return output;
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
