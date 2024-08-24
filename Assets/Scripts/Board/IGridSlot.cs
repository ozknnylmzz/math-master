using Math.Items;
using UnityEngine;

namespace Math.Boards
{
    public interface IGridSlot
    {
        public bool CanSetItem { get; }
        
        public int ItemId { get; }
                
        public bool HasItem { get; }

        public bool CanContainItem { get; }

        public GridPosition GridPosition { get; }

        public Vector3 WorldPosition { get; }

        public GridItem Item { get; }

        public void SetItem(GridItem item);

        public void ClearSlot();

        public void SetItemDrop(bool value);
     
    }
}