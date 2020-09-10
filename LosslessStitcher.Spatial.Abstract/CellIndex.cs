using System;
using System.Collections.Generic;
using HashCodeUtility;

namespace LosslessStitcher.Spatial
{
    using LosslessStitcher.Data;

    /// <summary>
    /// <see cref="CellIndex"/> represents the X and Y index of a cell in a grid.
    /// 
    /// <para>
    /// <see cref="CellIndex"/> and <see cref="Point"/> are designed to be distinct, in order to 
    /// reduce programming mistakes in algorithms that operate on both grid cell coordinates and 
    /// image coordinates.
    /// </para>
    /// 
    /// <para>
    /// See also:
    /// <seealso cref="IGrid"/>,
    /// <seealso cref="Point"/>
    /// </para>
    /// </summary>
    public struct CellIndex
        : IEquatable<CellIndex>
    {
        public int CellX { get; }

        public int CellY { get; }

        public CellIndex(int cellX, int cellY)
        {
            CellX = cellX;
            CellY = cellY;
        }

        public CellIndex((int CellX, int CellY) ci)
        {
            CellX = ci.CellX;
            CellY = ci.CellY;
        }

        public CellIndex(Point point)
        {
            CellX = point.X;
            CellY = point.Y;
        }

        public override bool Equals(object obj)
        {
            if (obj is CellIndex other)
            {
                return this == other;
            }
            return false;
        }

        public bool Equals(CellIndex other)
        {
            return this == other;
        }

        public static bool operator ==(CellIndex ci1, CellIndex ci2)
        {
            return ci1.CellX == ci2.CellX && ci1.CellY == ci2.CellY;
        }

        public static bool operator !=(CellIndex ci1, CellIndex ci2)
        {
            return ci1.CellX != ci2.CellX || ci1.CellY != ci2.CellY;
        }

        public void Deconstruct(out int cellX, out int cellY)
        {
            cellX = CellX;
            cellY = CellY;
        }

        public Point ToPoint()
        {
            return new Point(CellX, CellY);
        }

        public static explicit operator CellIndex(Point p)
        {
            return new CellIndex(p.X, p.Y);
        }

        public static explicit operator Point(CellIndex ci)
        {
            return new Point(ci.CellX, ci.CellY);
        }

        public static explicit operator CellIndex((int CellX, int CellY) ci)
        {
            return new CellIndex(ci.CellX, ci.CellY);
        }

        public static explicit operator (int CellX, int CellY)(CellIndex ci)
        {
            return (ci.CellX, ci.CellY);
        }
        public override int GetHashCode()
        {
            return HashCodeBuilder.ForType<CellIndex>().Ingest(CellX, CellY).GetHashCode();
        }

        public override string ToString()
        {
            return $"(cell: {CellX}, {CellY})";
        }

        public struct CompareRaster : IComparer<CellIndex>
        {
            public int Compare(CellIndex ci1, CellIndex ci2)
            {
                if (ci1.CellY > ci2.CellY) return 1;
                if (ci1.CellY < ci2.CellY) return -1;
                if (ci1.CellX > ci2.CellX) return 1;
                if (ci1.CellX < ci2.CellX) return -1;
                return 0;
            }
        }
    }
}
