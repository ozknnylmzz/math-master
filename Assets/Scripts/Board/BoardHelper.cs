using System.Collections;
using System.Collections.Generic;
using Math.Matchs;
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
        
        public static bool GetAllEmptySlotsBelow(IGridSlot currentSlot, IBoard board, out ItemFallData fallData)
        {
            // Boş slotları ekleyeceğimiz liste
            List<IGridSlot> emptySlots = new List<IGridSlot>();
            GridPosition currentPosition = currentSlot.GridPosition;

            // İlk olarak bir slot alttaki pozisyonu kontrol et
            GridPosition belowPosition = new GridPosition(currentPosition.RowIndex - 1, currentPosition.ColumnIndex);

            // Döngü ile alt slotları kontrol et
            while (board.IsPositionInBounds(belowPosition))
            {
                IGridSlot belowSlot = board[belowPosition.RowIndex, belowPosition.ColumnIndex];

                // Eğer alt slot boşsa listeye ekle, boş değilse döngüden çık
                if (belowSlot.CanContainItem && belowSlot.Item == null)
                {
                    emptySlots.Add(belowSlot);
                }
                else
                {
                    // İlk dolu slotu bulduğumuzda döngüyü sonlandırıyoruz
                    break;
                }

                // Bir alt pozisyona geç
                belowPosition = new GridPosition(belowPosition.RowIndex - 1, currentPosition.ColumnIndex);
            }

            // Eğer boş slotlar varsa, en son boş slotu target olarak seçelim
            if (emptySlots.Count > 0)
            {
                IGridSlot targetSlot = emptySlots[emptySlots.Count - 1]; // En alttaki boş slot hedef slot olacak
                int pathDistance = currentPosition.RowIndex - targetSlot.GridPosition.RowIndex; // Yükseklik farkı (kaç slot aşağıya gidecek)

                // ItemFallData'yı oluştur ve out parametresi olarak ver
                fallData = new ItemFallData(currentSlot.Item, targetSlot, pathDistance);
                return true;
            }

            // Eğer boş slot yoksa fallData null olur
            fallData = null;
            return false;
        }
        
        public static GridPosition CalculateSlotBasedMovement(IBoard board,GridPosition currentGridPosition, Vector2 pointerWorldPos)
        {
            // Sağdaki slotu kontrol et
            GridPosition rightPosition = currentGridPosition + GridPosition.Right;
            if (board.IsPositionInBounds(rightPosition) && board[rightPosition].HasItem)
            {
                return rightPosition;
            }

            // Soldaki slotu kontrol et
            GridPosition leftPosition = currentGridPosition + GridPosition.Left;
            if (board.IsPositionInBounds(leftPosition) && board[leftPosition].HasItem)
            {
                return leftPosition;
            }

            // Eğer sağ ve solunda item yoksa mevcut pozisyonu geri döndür
            return currentGridPosition;
        }

    }
}
