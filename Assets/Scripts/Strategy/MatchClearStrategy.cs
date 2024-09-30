using System.Collections.Generic;
using Math.Boards;
using Math.Items;
using Math.Matchs;

namespace Match3.Strategy
{
    public class MatchClearStrategy
    {
        private readonly BoardClearStrategy _boardClearStrategy;

        private readonly HashSet<IGridSlot> _matchSlots = new HashSet<IGridSlot>();
        private readonly HashSet<GridItem> _matchItems = new HashSet<GridItem>();

        public MatchClearStrategy(BoardClearStrategy boardClearStrategy)
        {
            _boardClearStrategy = boardClearStrategy;
        }

        public void CalculateMatchStrategyJobs(BoardDropItemData dropItemData,GridItem selectedGridItem,GridItem targetGridItem)
        {
            _matchItems.Clear();
            _matchSlots.Clear();

            _matchItems.UnionWith(new[] { selectedGridItem, targetGridItem });
            _matchSlots.UnionWith(new[] { selectedGridItem.ItemSlot, targetGridItem.ItemSlot });

            _boardClearStrategy.ClearAllSlots(_matchSlots,_matchItems);
            _boardClearStrategy.Refill(dropItemData,targetGridItem.ItemSlot,targetGridItem);
        }

    }
}