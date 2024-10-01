using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Math.Boards
{
    public static class BoardExtensions 
    {
        public static bool CanMoveDown(this IBoard board, IGridSlot currentSlot, out GridPosition bottomPosition)
        {
            IGridSlot bottomSlot = board.GetSlotInDirection(currentSlot, GridPosition.Down);

            if (bottomSlot is { CanSetItem: true })
            {
                bottomPosition = bottomSlot.GridPosition;
                return true;
            }
            
            bottomPosition = GridPosition.Zero;
            return false;
        }
        
        private static IGridSlot GetSlotInDirection(this IBoard board, IGridSlot currentSlot, GridPosition direction)
        {
            GridPosition nextSlotPosition = currentSlot.GridPosition + direction;

            if (board.IsPositionInBounds(nextSlotPosition))
            {
                return board[nextSlotPosition];
            }

            return null;
        }
    }
}
