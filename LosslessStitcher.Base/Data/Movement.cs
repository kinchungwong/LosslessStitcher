using System;
using HashCodeUtility;

namespace LosslessStitcher.Data
{
    public struct Movement
        : IEquatable<Movement>
    {
        public static Movement Zero { get; } = new Movement(0, 0);

        public int DeltaX { get; }

        public int DeltaY { get; }

        public Movement(int deltaX, int deltaY)
        {
            DeltaX = deltaX;
            DeltaY = deltaY;
        }

        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case Movement other:
                    return this == other;
                default:
                    return false;
            }
        }

        public bool Equals(Movement other)
        {
            return this == other;
        }

        public static bool operator ==(Movement m1, Movement m2)
        {
            return m1.DeltaX == m2.DeltaX &&
                m1.DeltaY == m2.DeltaY;
        }

        public static bool operator !=(Movement m1, Movement m2)
        {
            return m1.DeltaX != m2.DeltaX ||
                m1.DeltaY != m2.DeltaY;
        }

        public static Movement operator +(Movement m1, Movement m2)
        {
            return new Movement(m1.DeltaX + m2.DeltaX, m1.DeltaY + m2.DeltaY);
        }

        public static Movement operator -(Movement m1, Movement m2)
        {
            return new Movement(m1.DeltaX - m2.DeltaX, m1.DeltaY - m2.DeltaY);
        }

        public override int GetHashCode()
        {
            return HashCodeBuilder.ForType<Movement>().Ingest(DeltaX, DeltaY).GetHashCode();
        }

        public override string ToString()
        {
            return $"(dx={DeltaX}, dy={DeltaY})";
        }

    }
}
