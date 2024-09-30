using System.Collections.Generic;
using Math.Boards;
using Math.Enums;
using Math.Items;

namespace Math.Matchs
{
    public class ItemFallData
    {
        public GridItem Item { get; private set; }
        public IGridSlot DestinationSlot { get; }
        
        public ItemFallData(GridItem item, IGridSlot destinationSlot)
        {
            Item = item;
            DestinationSlot = destinationSlot;
        }
    } 
}