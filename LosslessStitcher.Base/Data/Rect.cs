using System;
using HashCodeUtility;

namespace LosslessStitcher.Data
{
    public struct Rect
        : IEquatable<Rect>
    {
        public static Rect Zero => new Rect(0, 0, 0, 0);

        public int X { get; }

        public int Y { get; }

        public int Width { get; }

        public int Height { get; }

        public int Left => X;

        public int Top => Y;

        public int Right => X + Width;

        public int Bottom => Y + Height;

        public Point TopLeft => new Point(Left, Top);

        public Point TopRight => new Point(Right, Top);

        public Point BottomLeft => new Point(Left, Bottom);

        public Point BottomRight => new Point(Right, Bottom);

        public Size Size => new Size(Width, Height);

        /// <summary>
        /// Both Width and Height are positive.
        /// </summary>
        public bool IsPositive => Width > 0 && Height > 0;

        /// <summary>
        /// Both Width and Height are non-negative.
        /// </summary>
        public bool IsNonNegative => Width >= 0 && Height >= 0;

        /// <summary>
        /// Some of the parameters are negative.
        /// </summary>
        public bool IsNegative => Width < 0 || Height < 0;

        /// <summary>
        /// Both Width and Height are zero.
        /// </summary>
        public bool IsEmpty => Width == 0 && Height == 0;

        public Rect(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Rect(Point topLeft, Size size)
        {
            X = topLeft.X;
            Y = topLeft.Y;
            Width = size.Width;
            Height = size.Height;
        }

        public override bool Equals(object obj)
        {
            if (obj is Rect other)
            {
                return this == other;
            }
            return false;
        }

        public bool Equals(Rect other)
        {
            return this == other;
        }

        public static bool operator ==(Rect p1, Rect p2)
        {
            return p1.X == p2.X && p1.Y == p2.Y &&
                p1.Width == p2.Width && p1.Height == p2.Height;
        }

        public static bool operator !=(Rect p1, Rect p2)
        {
            return p1.X != p2.X || p1.Y != p2.Y ||
                p1.Width != p2.Width || p1.Height != p2.Height;
        }

        public static Rect operator +(Rect r, Movement m)
        {
            return new Rect(r.X + m.DeltaX, r.Y + m.DeltaY, r.Width, r.Height);
        }

        public static Rect operator -(Rect r, Movement m)
        {
            return new Rect(r.X - m.DeltaX, r.Y - m.DeltaY, r.Width, r.Height);
        }

        public override int GetHashCode()
        {
            return HashCodeBuilder.ForType<Rect>().Ingest(X, Y, Width, Height).GetHashCode();
        }

        public override string ToString()
        {
            return $"(X={X}, Y={Y}, W={Width}, H={Height})";
        }
    }
}
