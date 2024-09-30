using System.Collections.Generic;
using Math.Boards;
using Math.Items;

namespace Math.Matchs
{
    public class MatchDataProvider : IMatchDataProvider
    {
        private BoardDropItemData _dropDataAllSlots = new();

        public MatchDataProvider()
        {
        }

        public BoardDropItemData GetMatchData(IBoard board, GridPosition selectedPosition, GridPosition targetPosition)
        {
            // Selected pozisyonunu boş bırakıyoruz ve üstündeki item'ları aşağı kaydırıyoruz
            HandleSelectedItemDrop(board, selectedPosition);

            // Target pozisyonunda yeni bir item oluştu, bu item'ı kontrol ediyoruz
            HandleTargetItemMatch(board, targetPosition);

            return _dropDataAllSlots;
        }

        /// <summary>
        /// Selected pozisyonundaki item boşaldığında, üstteki item'ları aşağıya kaydırır.
        /// Her item düştüğünde, eşleşme olup olmadığını kontrol eder ve gerekiyorsa yeni eşleşmeleri işler.
        /// </summary>
        private void HandleSelectedItemDrop(IBoard board, GridPosition selectedPosition)
        {
            // Selected pozisyonunun üstündeki item'lar sırayla aşağıya düşmeli
            int row = selectedPosition.RowIndex - 1;
            while (row >= 0)
            {
                GridPosition abovePosition = new GridPosition(row, selectedPosition.ColumnIndex);
                IGridSlot aboveSlot = board[abovePosition];
                IGridSlot targetSlot = board[selectedPosition];
                GridItem aboveItem = board.GetNormalItem(abovePosition);

                // Eğer üstte bir item varsa ve alttaki slot boşsa item'ı aşağıya kaydır
                if (aboveSlot.Item != null && targetSlot.Item == null)
                {
                    ItemFallData itemFallData = new ItemFallData(aboveItem, targetSlot);
                    _dropDataAllSlots.AddDropData(itemFallData);

                    // // Üst item'i alttaki boş slot'a taşıyoruz
                    targetSlot.SetItem(aboveSlot.Item);
                    aboveSlot.ClearSlot();

                    // Yeni düşen item'de eşleşme olup olmadığını kontrol et
                    HandleTargetItemMatch(board, targetSlot.GridPosition);
                }

                // Bir üst satıra geçiyoruz
                selectedPosition = abovePosition;
                row--;
            }
        }

        /// <summary>
        /// Target pozisyonuna gelen item'i kontrol eder. Eşleşme varsa, yeni eşleşmelerin üstündeki item'ları aşağı kaydırır.
        /// </summary>
        private void HandleTargetItemMatch(IBoard board, GridPosition targetPosition)
        {
            // Target pozisyonundaki item ile altındaki item arasında eşleşme olup olmadığını kontrol et
            if (BoardHelper.IsItemBelow(board[targetPosition], board, out IGridSlot belowSlot))
            {
                if (board[targetPosition].Item.ColorType == belowSlot.Item.ColorType)
                {
                    // Eşleşme var, target pozisyonuna bir drop işlemi ekle
                    ItemFallData itemFallData = new ItemFallData(belowSlot.Item, board[targetPosition]);
                    _dropDataAllSlots.AddDropData(itemFallData);

                    // // Target pozisyonundaki item'i alttaki pozisyona taşı
                    belowSlot.SetItem(board[targetPosition].Item);

                    // Eşleşme sonrasında item'in ColorType'ını 1 artırıyoruz
                    board[targetPosition].Item
                        .SetColorType(board[targetPosition].Item.ColorType + 1); // ColorType'ı bir artırma işlemi

                    // Target pozisyonundaki item'i temizle
                    board[targetPosition].ClearSlot();

                    // Altındaki item'den eşleşmeyi tekrar kontrol et ve zincirleme eşleşmeler için üstten item düşür
                    HandleSelectedItemDrop(board, targetPosition);
                }
            }
        }
    }

    // public class DropDataAllSlots
    // {
    //     public List<BoardDropItemData> MatchDataList;
    //     public HashSet<IGridSlot> AllMatchedGridSlots;
    //
    //     public DropDataAllSlots()
    //     {
    //         MatchDataList = new List<BoardDropItemData>();
    //         AllMatchedGridSlots = new HashSet<IGridSlot>();
    //     }
    // }
}