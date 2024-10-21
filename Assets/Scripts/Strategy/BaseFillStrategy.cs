using System.Collections.Generic;
using DG.Tweening;
using Math.Boards;
using Math.Items;
using Math.Matchs;

namespace Math.Strategy
{
    public abstract class BaseFillStrategy 
    {
        protected readonly IBoard _board;
        protected readonly ItemGenerator _itemGenerator;

        protected BaseFillStrategy(IBoard board, ItemGenerator itemGenerator)
        {
            _board = board;
            _itemGenerator = itemGenerator;
        }

        public abstract Tween AddFillJobs(IGridSlot selectedSlot,IGridSlot matchSlot,GridItem gridItem);
    }
}