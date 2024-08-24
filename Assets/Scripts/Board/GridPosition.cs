using System;

namespace Math.Boards
{
    [Serializable]
    public struct GridPosition :IEquatable<GridPosition>
    {
        public GridPosition(int rowIndex, int columnIndex)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
        }

        public int RowIndex { get; }
        public int ColumnIndex { get; }

        #region Shorthands

        /// <summary>
        ///   <para>Shorthand for writing GridPosition(-1, 0).</para>
        /// </summary>
        public static GridPosition Up { get; } = new GridPosition(1, 0);

        /// <summary>
        ///   <para>Shorthand for writing GridPosition(1, 0).</para>
        /// </summary>
        public static GridPosition Down { get; } = new GridPosition(-1, 0);

        /// <summary>
        ///   <para>Shorthand for writing GridPosition(0, -1).</para>
        /// </summary>
        public static GridPosition Left { get; } = new GridPosition(0, -1);

        /// <summary>
        ///   <para>Shorthand for writing GridPosition(0, 1).</para>
        /// </summary>
        public static GridPosition Right { get; } = new GridPosition(0, 1);

        /// <summary>
        ///   <para>Shorthand for writing GridPosition(0, 0).</para>
        /// </summary>
        public static GridPosition Zero { get; } = new GridPosition(0, 0);

        public static readonly GridPosition[] SideDirections = new GridPosition[]
            { Up, Down, Right, Left };

        public static readonly GridPosition[] DiagonalDirections = new GridPosition[]
            { Up + Right, Down + Right, Up + Left, Down + Left };

        #endregion

        #region Operators

        public static GridPosition operator +(GridPosition a, GridPosition b)
        {
            return new GridPosition(a.RowIndex + b.RowIndex, a.ColumnIndex + b.ColumnIndex);
        }

        public static GridPosition operator -(GridPosition a, GridPosition b)
        {
            return new GridPosition(a.RowIndex - b.RowIndex, a.ColumnIndex - b.ColumnIndex);
        }

        public static GridPosition operator *(int multiply, GridPosition a)
        {
            return new GridPosition(a.RowIndex * multiply, a.ColumnIndex * multiply);
        }

        public static GridPosition operator *(GridPosition a, int multiply)
        {
            return new GridPosition(a.RowIndex * multiply, a.ColumnIndex * multiply);
        }

        public static bool operator ==(GridPosition a, GridPosition b)
        {
            return a.RowIndex == b.RowIndex && a.ColumnIndex == b.ColumnIndex;
        }

        public static bool operator !=(GridPosition a, GridPosition b)
        {
            return a.RowIndex != b.RowIndex || a.ColumnIndex != b.ColumnIndex;
        }

        public bool Equals(GridPosition other)
        {
            return RowIndex == other.RowIndex && ColumnIndex == other.ColumnIndex;
        }

        public override bool Equals(object obj)
        {
            return obj is GridPosition other && Equals(other);
        }

        public override int GetHashCode()
        {
            return RowIndex.GetHashCode() ^ (ColumnIndex.GetHashCode() << 2);
        }

        public override string ToString()
        {
            return "(" + RowIndex + "," + ColumnIndex + ")";
        }

        #endregion

        #region Methods

        public bool IsPositionInBounds(int rowCount, int columnCount)
        {
            return RowIndex >= 0 &&
                   RowIndex < rowCount &&
                   ColumnIndex >= 0 &&
                   ColumnIndex < columnCount;
        }

        #endregion
    }
}