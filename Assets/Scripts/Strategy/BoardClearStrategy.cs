using System.Collections.Generic;
using Math.Boards;
using Math.Items;
using Math.Matchs;
using Math.Strategy;

namespace Match3.Strategy
{
    public class BoardClearStrategy
    {
        private readonly BaseFillStrategy _fillStrategy;

        public BoardClearStrategy(BaseFillStrategy fillStrategy)
        {
            _fillStrategy = fillStrategy;
        }

        public void Refill(IGridSlot selectedSlot,IGridSlot matchSlot,GridItem gridItem)
        {
            _fillStrategy.AddFillJobs(selectedSlot,matchSlot,gridItem);
        }

        public void ClearAllSlots(IEnumerable<IGridSlot> allSlots,IEnumerable<GridItem> gridItems)
        {
            foreach (IGridSlot slot in allSlots)
            {
                slot.Item.ReturnToPool();
                slot.ClearSlot();
                
            }

            foreach (GridItem gridItem in gridItems)
            {
                gridItem.Hide();   
            }
        }
    }
}