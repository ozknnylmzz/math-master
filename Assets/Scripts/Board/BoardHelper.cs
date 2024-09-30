using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Math.Boards
{
    public class BoardHelper : MonoBehaviour
    {
        public static bool IsItemBelow(IGridSlot currentSlot, IBoard board,out IGridSlot belowSlot)
        {
            GridPosition currentPosition = currentSlot.GridPosition;

            GridPosition belowPosition = new GridPosition(currentPosition.RowIndex -1, currentPosition.ColumnIndex);

            if (board.IsPositionInBounds(belowPosition))
            {
                 belowSlot = board[belowPosition.RowIndex, belowPosition.ColumnIndex];

                return belowSlot.CanContainItem && belowSlot.Item != null;
            }

            belowSlot = null;
            
            return false;
        }
    }
}
