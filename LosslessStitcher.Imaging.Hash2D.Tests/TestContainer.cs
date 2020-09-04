using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace LosslessStitcher.Imaging.Hash2D.Tests
{
    using HashCodeUtility;
    using LosslessStitcher.Data;
    using LosslessStitcher.Functional;
    using LosslessStitcher.Imaging.Hash2D.Simple;
    using LosslessStitcher.Imaging.IO;

    public class TestContainer
    {
        public IContainer Container;

        public TestContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(new BitmapFactory()).As<IBitmapFactory>();
            builder.RegisterType<SimpleHash2DProcessor>().As<IHash2DProcessor>();
            builder.RegisterType<BitmapCodecManager>().As<IBitmapCodecManager>();
            builder.RegisterType<SimpleUniqueHashFilter>().As<IUniqueHashFilter>();
            Container = builder.Build();
        }
    }
}
