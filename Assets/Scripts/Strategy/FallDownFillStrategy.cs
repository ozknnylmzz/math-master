using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Match3.Strategy;
using Math.Boards;
using Math.Enums;
using Math.Items;
using Math.Matchs;
using Math.Strategy;
using UnityEngine;

namespace Math.Strategy
{
    public class FallDownFillStrategy : BaseFillStrategy
    {
        private int oldColumnIndex = -1;
        private int extraRowIndex;

        private ItemGenerator _itemGenerator;

        public FallDownFillStrategy(IBoard board, ItemGenerator itemGenerator) : base(board, itemGenerator)
        {
            _itemGenerator = itemGenerator;
        }

        private void ShowMatch(ItemFallData itemFallData)
        {
            // if (itemFallData.IsMatch)
            // {
            //     GridItem matchItem = _itemGenerator.GetMatchItem(itemFallData.GridItem.ColorType-1);
            //     _itemGenerator.SetItemOnSlot(matchItem,itemFallData.TargetGridSlot);
            // }
        }

        public override void AddFillJobs(IGridSlot selectedSlot, IGridSlot matchSlot, GridItem gridItem)
        {
            GridItem matchItem = _itemGenerator.GetMatchItem(gridItem.ColorType - 1);
            _itemGenerator.SetItemOnSlot(matchItem, matchSlot);

            DropItemsInColumn(_board, selectedSlot.GridPosition, matchSlot.GridPosition);

            #region MyRegion

            // foreach (var dropData in boardDropItemData.DropDatas)
            // {
            //     dropData.DropGridSlot.Item.DoMove(dropData.TargetGridSlot).OnComplete(() => ShowMatch(dropData));
            // }

            // if (!BoardHelper.IsItemBelow(matchSlot, _board,out IGridSlot targetSlot))
            // {
            //     gridItem.DoMove(targetSlot);
            // }

            // IEnumerable<int> fallSlotsColumnIndexes = allSlots.Select(slot => slot.GridPosition.ColumnIndex);
            // List<ItemFallData> allItemsFallData = new();
            //
            // ResetDropSlots();
            //
            // foreach (int columnIndex in fallSlotsColumnIndexes)
            // {
            //     IEnumerable<GridItem> itemsToHideOnColumn = GetItemsOnColumn(allItems, columnIndex);
            //     List<ItemFallData> itemsFallData = GetItemsFallDataPerColumn(columnIndex);
            //
            //     allItemsFallData.AddRange(itemsFallData);
            //
            //     if (itemsFallData.Count != 0)
            //     {
            //         JobsExecutor.AddFallJob(new ItemsFallJob(itemsFallData, itemsToHideOnColumn), columnIndex);
            //     }
            // }

            #endregion
        }

        #region MyRegion

        // private List<ItemFallData> GetItemsFallDataPerColumn(int columnIndex)
        // {
        //     List<ItemFallData> itemsFallData = new List<ItemFallData>();
        //
        //     DropItemsOnBoard(_board, columnIndex, itemsFallData);
        //
        //     DropItemsAboveBoard(_board, columnIndex, itemsFallData);
        //
        //     oldColumnIndex = -1;
        //     itemsFallData.Reverse();
        //
        //     return itemsFallData;
        // }

        // private void DropItemsOnBoard(IBoard board, int columnIndex, List<ItemFallData> itemsFallData)
        // {
        //     for (int rowIndex = 0; rowIndex < board.RowCount; rowIndex++)
        //     {
        //         IGridSlot currentSlot = board[rowIndex, columnIndex];
        //
        //         if (!currentSlot.HasItem)
        //         {
        //             continue;
        //         }
        //
        //         if (!CanDropDown(board, currentSlot, out GridPosition destinationPosition))
        //         {
        //             continue;
        //         }
        //
        //         GridItem item = currentSlot.Item;
        //
        //         currentSlot.ClearSlot();
        //
        //         IGridSlot destinationSlot = board[destinationPosition];
        //
        //         destinationSlot.SetItem(item);
        //
        //         int pathDistance = rowIndex - destinationPosition.RowIndex;
        //
        //         item.SetDestinationSlot(destinationSlot);
        //         item.SetState(ItemState.WaitingToFall);
        //
        //         itemsFallData.Add(new ItemFallData(item, destinationSlot, pathDistance));
        //     }
        // }
        //
        // private void DropItemsAboveBoard(IBoard board, int columnIndex, List<ItemFallData> itemsFallData)
        // {
        //     for (int rowIndex = 0; rowIndex < board.RowCount; rowIndex++)
        //     {
        //         IGridSlot currentSlot = board[rowIndex, columnIndex];
        //
        //         if (!currentSlot.CanSetItem)
        //         {
        //             continue;
        //         }
        //
        //         GridPosition abovePosition = GetFallPositionAboveBoard(board, columnIndex);
        //
        //         GridItem item = _itemGenerator.GetRandomNormalItem();
        //
        //         item.SetWorldPosition(board.GridToWorldPosition(abovePosition));
        //
        //         currentSlot.SetItem(item);
        //         currentSlot.SetItemDrop(true);
        //
        //         int pathDistance = abovePosition.RowIndex - rowIndex;
        //
        //         item.SetDestinationSlot(currentSlot);
        //         item.SetState(ItemState.WaitingToFall);
        //
        //         itemsFallData.Add(new ItemFallData(item, currentSlot, pathDistance));
        //     }
        // }

        #endregion

        private Tween DropItemsInColumn(IBoard board, GridPosition selectedGridPosition, GridPosition matchGridPosition)
        {
            Sequence dropSequence = DOTween.Sequence(); // Tüm hareketleri bir sequence'e ekleyeceğiz
            int columnIndex = selectedGridPosition.ColumnIndex;

            // Yukarıdan aşağıya doğru tara
            for (int rowIndex = 0; rowIndex < board.RowCount; rowIndex++)
            {
                GridPosition currentPos = new GridPosition(rowIndex, columnIndex);
                IGridSlot currentSlot = board[currentPos];

                // Eğer slot boşsa, hedef slot bu slot olsun ve üstündeki item'ları listele
                if (!currentSlot.HasItem)
                {
                    List<GridItem> dropItems = new();

                    // Bu boş slotun üstündeki item'ları listele (yukarıdan aşağıya sırayla)
                    for (int i = rowIndex + 1; i < board.RowCount; i++)
                    {
                        GridPosition abovePosition = new GridPosition(i, columnIndex);
                        IGridSlot aboveSlot = board[abovePosition];

                        if (aboveSlot.HasItem)
                        {
                            dropItems.Add(aboveSlot.Item); // Item'ı listeye ekle
                            aboveSlot.ClearSlot(); // Slotu temizle
                        }
                    }

                    // Şimdi listedeki item'ları birer alt slota kaydır
                    foreach (GridItem item in dropItems)
                    {
                        // İlk boş slot (target slot) ve pozisyon
                        IGridSlot targetSlot = board[currentPos];
                        Vector3 startPosition =
                            board.GridToWorldPosition(item.ItemSlot.GridPosition); // Başlangıç pozisyonu
                        Vector3 targetPosition = board.GridToWorldPosition(targetSlot.GridPosition); // Hedef pozisyon

                        // Önce item'ı set et
                        targetSlot.SetItem(item);

                        // DOMove ile item'ı animasyonla hareket ettir
                        item.transform.position = startPosition; // Başlangıç pozisyonuna geri döndür
                        if (IsMatchItem(_board,item.ItemSlot,out IGridSlot gridSlot))
                        {
                            GridItem matchItem = _itemGenerator.GetMatchItem(item.ColorType - 1);
                            _itemGenerator.SetItemOnSlot(matchItem, gridSlot);
                        }
                        dropSequence.Append(item.transform.DOMove(targetPosition, 0.5f).OnComplete(() =>
                        {
                           
                        }));

                        // Bir sonraki item için boş slotu güncelle
                        currentPos = new GridPosition(currentPos.RowIndex + 1, columnIndex);
                    }

                    break; // Boş bir slot bulunduğu anda işlemi sonlandır
                }
            }

            return dropSequence;
        }

        private HashSet<GridItem> GetItemsOnColumn(IEnumerable<GridItem> allItems, int columnIndex)
        {
            HashSet<GridItem> matchItemsOnColumn = new();

            foreach (GridItem item in allItems)
            {
                GridPosition itemPosition = item.ItemSlot.GridPosition;

                if (itemPosition.ColumnIndex == columnIndex)
                {
                    matchItemsOnColumn.Add(item);
                }
            }

            return matchItemsOnColumn;
        }

        private void ResetDropSlots()
        {
            foreach (IGridSlot slot in _board)
            {
                slot.SetItemDrop(false);
            }
        }

        private GridPosition GetFallPositionAboveBoard(IBoard board, int columnIndex)
        {
            if (columnIndex == oldColumnIndex)
            {
                extraRowIndex++;
            }
            else
            {
                extraRowIndex = 0;
            }

            oldColumnIndex = columnIndex;

            return new GridPosition(board.RowCount + extraRowIndex, columnIndex);
        }

        private bool CanDropDown(IBoard board, IGridSlot gridSlot, out GridPosition destinationPosition)
        {
            IGridSlot destinationSlot = gridSlot;

            while (board.CanMoveDown(destinationSlot, out GridPosition bottomPosition))
            {
                destinationSlot = board[bottomPosition];
            }

            destinationPosition = destinationSlot.GridPosition;

            return destinationSlot != gridSlot;
        }

        private bool IsMatchItem(IBoard board, IGridSlot gridSlot, out IGridSlot destinationSlot)
        {
            destinationSlot = null; // Varsayılan olarak null

            // Eğer gridSlot boşsa, hiçbir işlem yapmaya gerek yok
            if (!gridSlot.HasItem)
            {
                return false;
            }

            // Altında bir slot var mı kontrol edelim
            if (board.CanMoveDown(gridSlot, out GridPosition bottomPosition))
            {
                IGridSlot bottomSlot = board[bottomPosition];

                // Eğer alttaki slotta item varsa ve aynı ColorType'taysa
                if (bottomSlot.HasItem && bottomSlot.Item.ColorType == gridSlot.Item.ColorType)
                {
                    destinationSlot = bottomSlot;
                    return true; // Aynı renkte item bulundu
                }

                // Alttaki slot boşsa veya renk farklıysa
                destinationSlot = bottomSlot; // Boş slotu dönelim
                return false;
            }

            // Eğer altındaki slot mevcut değilse
            return false;
        }
    }
}