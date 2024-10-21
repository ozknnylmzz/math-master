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
        
        public int PathDistance { get; }
        
        public ItemFallData(GridItem item, IGridSlot destinationSlot,int pathDistance)
        {
            Item = item;
            DestinationSlot = destinationSlot;
            PathDistance = pathDistance;
            // item.SetPathDistance(pathDistance);
        }
    } 
}