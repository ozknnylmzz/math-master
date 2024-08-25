using System.Collections.Generic;
using Math.Boards;
using Math.Enums;
using UnityEngine;

namespace Math.Items
{
    public class ItemGenerator : MonoBehaviour
    {
        private readonly Dictionary<ItemType, ObjectPool<GridItem>> _itemPools = new();

        private int[] _possibleConfigureTypes;

        public void GeneratePool(GridItem prefab, int itemPoolSize)
        {
            _itemPools[prefab.ItemType] = new ObjectPool<GridItem>(prefab, itemPoolSize, transform, InitializeItem);
        }

        public void SetConfigureTypes(int[] possibleConfigureTypes)
        {
            _possibleConfigureTypes = possibleConfigureTypes;
        }

        private GridItem GetItemWithId(ItemType itemType, int configureType = 0)
        {
            GridItem item = _itemPools[itemType].GetFromPool();

            ConfigureItem(item, configureType);
            return item;
        }

        public GridItem GetRandomNormalItem()
        {
            return GetItemWithId(ItemType.BoardItem, _possibleConfigureTypes.ChooseRandom());
        }

        public void ReturnItemToPool(GridItem item)
        {
            _itemPools[item.ItemType].ReturnToPool(item);
        }

        public void SetItemOnSlot(GridItem item, IGridSlot slot)
        {
            item.SetWorldPosition(slot.WorldPosition);

            slot.SetItem(item);
        }

        private void ConfigureItem(GridItem item, int configureType)
        {
            item.ConfigureItem(configureType);
            item.ResetItem();
        }

        private void InitializeItem(GridItem item)
        {
            item.SetGenerator(this);
            item.Initialize();
        }
    }
}