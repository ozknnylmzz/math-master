using System.Collections.Generic;
using Math.Items;
using UnityEngine;

namespace Math.Boards
{
    public interface IBoard : IEnumerable<IGridSlot>
    {
        public int RowCount { get; }
        public int ColumnCount { get; }
        public float LeftBoundX { get;  }
        public float RightBoundX { get; }

        public IGridSlot this[GridPosition gridPosition] { get; }
        public IGridSlot this[int rowIndex, int columnIndex] { get; }

        public GridPosition[] AllGridPositions { get; }

        public bool IsPointerOnBoard(Vector3 pointerWorldPos, out GridPosition gridPosition);

        public bool IsPositionInBounds(GridPosition gridPosition);

        public bool IsPositionOnItem(GridPosition gridPosition);
        public GridItem GetGridItem(Vector3 pointerWorldPos);
        public GridItem GetNormalItem(GridPosition gridPosition);
        public IGridSlot GetGridSlot(Vector3 pointerWorldPos);
        public Vector3 GridToWorldPosition(GridPosition gridPosition);
        public float GetTopSlotY();
        public float GetBottomSlotY();
        public float GetLeftSlotX();
        public float GetRightSlotX();
    } 
}