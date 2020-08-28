using System;
using HashCodeUtility;

namespace LosslessStitcher.Data
{
    public struct Point
        : IEquatable<Point>
    {
        public static Point Origin => new Point(0, 0);

        public int X { get; }

        public int Y { get; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj is Point other)
            {
                return this == other;
            }
            return false;
        }

        public bool Equals(Point other)
        {
            return this == other;
        }

        public static bool operator ==(Point p1, Point p2)
        {
            return p1.X == p2.X && p1.Y == p2.Y;
        }

        public static bool operator !=(Point p1, Point p2)
        {
            return p1.X != p2.X || p1.Y != p2.Y;
        }

        public void Deconstruct(out int x, out int y)
        {
            x = X;
            y = Y;
        }

        public static Movement operator -(Point argLeft, Point argRight)
        {
            return new Movement(argLeft.X - argRight.X, argLeft.Y - argRight.Y);
        }

        public static Point operator +(Point p, Movement m)
        {
            return new Point(p.X + m.DeltaX, p.Y + m.DeltaY);
        }

        public static Point operator -(Point p, Movement m)
        {
            return new Point(p.X - m.DeltaX, p.Y - m.DeltaY);
        }

        public override int GetHashCode()
        {
            return HashCodeBuilder.ForType<Point>().Ingest(X, Y).GetHashCode();
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}
