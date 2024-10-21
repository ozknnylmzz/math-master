using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
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

        public Tween CalculateMatchStrategyJobs(GridItem selectedGridItem,GridItem targetGridItem)
        {
            _matchItems.Clear();
            _matchSlots.Clear();

            _matchItems.UnionWith(new[] { selectedGridItem, targetGridItem });
            _matchSlots.UnionWith(new[] { selectedGridItem.ItemSlot, targetGridItem.ItemSlot });

            _boardClearStrategy.ClearAllSlots(_matchSlots,_matchItems);
          return  _boardClearStrategy.Refill(selectedGridItem .ItemSlot,targetGridItem.ItemSlot,targetGridItem);
        }

    }
}