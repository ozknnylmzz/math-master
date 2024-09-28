using System.Collections.Generic;
using System.Linq;
using Math.Boards;
using Math.Enums;
using Math.Items;
using Math.Strategy;

namespace Math.Strategy
{
    public class FillStrategy : BaseFillStrategy
    {
        private int oldColumnIndex = -1;
        private int extraRowIndex;

        private ItemGenerator _itemGenerator;

        public FillStrategy(IBoard board, ItemGenerator itemGenerator) : base(board, itemGenerator)
        {
            _itemGenerator = itemGenerator;
        }

        public override void AddFillJobs(IGridSlot matchSlot,GridItem gridItem)
        {
            GridItem matchItem = _itemGenerator.GetMatchItem(gridItem.ColorType);
            _itemGenerator.SetItemOnSlot(matchItem,matchSlot);
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
        }

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

        // private bool CanDropDown(IBoard board, IGridSlot gridSlot, out GridPosition destinationPosition)
        // {
        //     IGridSlot destinationSlot = gridSlot;
        //
        //     while (board.CanMoveDown(destinationSlot, out GridPosition bottomPosition))
        //     {
        //         destinationSlot = board[bottomPosition];
        //     }
        //
        //     destinationPosition = destinationSlot.GridPosition;
        //
        //     return destinationSlot != gridSlot;
        // }
    }
}