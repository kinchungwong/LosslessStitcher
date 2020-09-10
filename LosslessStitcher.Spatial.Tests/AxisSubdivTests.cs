using System;
using System.Collections.Generic;
using NUnit.Framework;
using Autofac;
using Autofac.Core;

namespace LosslessStitcher.Spatial.Tests
{
    using LosslessStitcher.Data;
    using LosslessStitcher.Spatial.Internals;

    public class AxisSubdivTests
    {
        public static IContainer Container;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<AxisSubdiv.MinAlign>();
            Container = builder.Build();
        }

        [Test]
        [TestCase(1, 1)]
        [TestCase(1, 2)]
        [TestCase(2, 1)]
        [TestCase(2, 2)]
        public void MinAlign_CtorTest_ShouldSucceed(int inputLength, int maxLength)
        {
            IAxisSubdiv axisSubdiv = new AxisSubdiv.MinAlign(inputLength, maxLength);
            Assert.Multiple(() =>
            {
                Assert.That(axisSubdiv.InputLength, Is.EqualTo(inputLength), "InputLength");
                Assert.That(axisSubdiv.MaxLength, Is.LessThanOrEqualTo(inputLength), "MaxLength");
                Assert.That(axisSubdiv.MaxLength, Is.LessThanOrEqualTo(maxLength), "MaxLength");
                Assert.That(axisSubdiv.MaxLength, Is.GreaterThanOrEqualTo(1), "MaxLength");
                Assert.That(axisSubdiv.MinLength, Is.LessThanOrEqualTo(axisSubdiv.MaxLength), "MinLength");
                Assert.That(axisSubdiv.MinLength, Is.GreaterThanOrEqualTo(1), "MinLength");
                Assert.That(axisSubdiv.Count, Is.GreaterThanOrEqualTo(1), "Count");
                Assert.That(axisSubdiv.InputRange.Start, Is.EqualTo(0), "InputRange.Start");
                Assert.That(axisSubdiv.InputRange.Count, Is.EqualTo(inputLength), "InputRange.Count");
                Assert.That(axisSubdiv[0].Start, Is.EqualTo(0), "Item[0].Start");
                Assert.That(axisSubdiv[axisSubdiv.Count - 1].Stop, Is.EqualTo(inputLength), "Item[axisSubdiv.Count - 1].Stop");
            });
        }

        [Test]
        [TestCase(-2, -2)]
        [TestCase(-1, -2)]
        [TestCase(0, -2)]
        [TestCase(1, -2)]
        [TestCase(-2, -1)]
        [TestCase(-1, -1)]
        [TestCase(0, -1)]
        [TestCase(1, -1)]
        [TestCase(-2, 0)]
        [TestCase(-1, 0)]
        [TestCase(0, 0)]
        [TestCase(1, 0)]
        [TestCase(-2, 1)]
        [TestCase(-1, 1)]
        [TestCase(0, 1)]
        public void MinAlign_InvalidCtorTest_ShouldThrow(int inputLength, int maxLength)
        {
            void TestFunc() 
            {
                new AxisSubdiv.MinAlign(inputLength, maxLength);
            }
            Assert.That(TestFunc, Throws.Exception);
        }
    }
}
