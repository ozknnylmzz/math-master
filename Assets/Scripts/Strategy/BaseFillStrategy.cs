using System.Collections.Generic;
using Math.Boards;
using Math.Items;

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

        public abstract void AddFillJobs(IGridSlot matchSlot,GridItem gridItem);
    }
}