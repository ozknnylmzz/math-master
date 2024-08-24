using UnityEngine;
using Math.Items;
namespace Math.Boards
{
    public class GridSlot : MonoBehaviour, IGridSlot
    {
        public bool CanSetItem => !HasItem;

        public int ItemId => (int)Item.ColorType;

        public bool HasItem => Item != null;

        public bool CanContainItem => true;

        public bool IsItemDroppedTo { get; private set; }

        public GridItem Item { get; private set; }

        public GridPosition GridPosition { get; private set; }

        public Vector3 WorldPosition { get; private set; }

        public void SetPosition(GridPosition gridPosition, Vector3 worldPosition)
        {
            GridPosition = gridPosition;
            WorldPosition = worldPosition;
        }

        public void SetItem(GridItem item)
        {
            Item = item;
            Item.SetSlot(this);
        }

        public void ClearSlot()
        {
            Item = default;
        }
        
        public void SetItemDrop(bool value)
        {
            IsItemDroppedTo = value;
        }
    } 
}