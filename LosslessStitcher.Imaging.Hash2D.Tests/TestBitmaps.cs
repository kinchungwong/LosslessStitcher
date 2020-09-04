namespace LosslessStitcher.Imaging.Hash2D.Tests
{
    using HashCodeUtility;
    using LosslessStitcher.Functional;

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
}
