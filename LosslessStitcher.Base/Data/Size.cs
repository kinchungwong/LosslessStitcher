using System;
using HashCodeUtility;

namespace LosslessStitcher.Data
{
    public struct Size
        : IEquatable<Size>
    {
        public int Width { get; }

        public int Height { get; }

        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override bool Equals(object obj)
        {
            if (obj is Size other)
            {
                return this == other;
            }
            return false;
        }

        public bool Equals(Size other)
        {
            return this == other;
        }

        public static bool operator ==(Size s1, Size s2)
        {
            return s1.Width == s2.Width && s1.Height == s2.Height;
        }

        public static bool operator !=(Size s1, Size s2)
        {
            return s1.Width != s2.Width || s1.Height != s2.Height;
        }

        public override int GetHashCode()
        {
            return HashCodeBuilder.ForType<Size>().Ingest(Width, Height).GetHashCode();
        }

        public override string ToString()
        {
            return $"(W={Width}, H={Height})";
        }
    }
}
