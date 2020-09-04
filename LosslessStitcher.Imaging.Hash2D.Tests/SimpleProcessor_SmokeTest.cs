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

    public class SimpleProcessor_SmokeTest
    {
        public IContainer Container;

        [SetUp]
        public void SetUp()
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(new BitmapFactory()).As<IBitmapFactory>();
            builder.RegisterType<SimpleHash2DProcessor>().As<IHash2DProcessor>();
            builder.RegisterType<BitmapCodecManager>().As<IBitmapCodecManager>();
            Container = builder.Build();
        }

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
            string ofn3 = $"{ofn1}_{ofn2}.png";
            string outputFilename = Path.Combine(Path.GetTempPath(), ofn3);
            _SaveBitmap(output, outputFilename);
        }

        public abstract class TestBitmapFunc
            : IFunc<int, int, int>
        {
            public abstract int Invoke(int x, int y);

            public virtual IArrayBitmap<int> Generate(IBitmapFactory bitmapFactory, int width, int height)
            {
                IArrayBitmap<int> bitmap = bitmapFactory.Create<int>(width, height);
                for (int y = 0; y < height; ++y)
                {
                    for (int x = 0; x < width; ++x)
                    {
                        bitmap[x, y] = Invoke(x, y);
                    }
                }
                return bitmap;
            }
        }

        public class TestBitmap_Blank : TestBitmapFunc
        {
            public override sealed int Invoke(int x, int y)
            {
                return 0;
            }
        }

        public class TestBitmap_TakeX : TestBitmapFunc
        {
            public override sealed int Invoke(int x, int y)
            {
                return x;
            }
        }

        public class TestBitmap_TakeY : TestBitmapFunc
        {
            public override sealed int Invoke(int x, int y)
            {
                return y;
            }
        }

        public class TestBitmap_SumXY : TestBitmapFunc
        {
            public override sealed int Invoke(int x, int y)
            {
                return x + y;
            }
        }

        public class TestBitmap_ProdXY : TestBitmapFunc
        {
            public override sealed int Invoke(int x, int y)
            {
                return x * y;
            }
        }

        public class TestBitmap_HashXY : TestBitmapFunc
        {
            public override sealed int Invoke(int x, int y)
            {
                return new HashCodeBuilder(0u).Ingest(x, y).GetHashCode();
            }
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
