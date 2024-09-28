using System.Threading.Tasks;
using Math.Boards;
using Math.Data;
using Math.Enums;
using Math.Items;
using UnityEngine;

namespace Math.Level
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private AllItemsData _allItemsData;

        private ItemGenerator _itemGenerator;
        // private MatchDataProvider _matchDataProvider;
        private IBoard _board;

        public async void Initialize(IBoard board, ItemGenerator itemGenerator) //GameConfig gameConfig)
        {
            _board = board;
            _itemGenerator = itemGenerator;
            SetConfigureTypes(Constants.CONFIGURETYPES_PIECE_VALUE_4);
            GenerateItemsPool(ItemType.BoardItem);
            await Task.Delay(1000);
            FillBoardWithItems();
        }

        public void SetConfigureTypes(int[] possibleConfigureTypes)
        {
            _itemGenerator.SetConfigureTypes(possibleConfigureTypes);
        }

        public void FillBoardWithItems()
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < _board.ColumnCount; j++)
                {
                    IGridSlot gridSlot = _board[i, j];

                    if (!gridSlot.CanSetItem)
                        continue;

                    SetItemWithoutMatch(gridSlot);
                }
            }
        }

        public void GenerateItemsPool(ItemType itemType)
        {
            ItemData itemData = _allItemsData.GetItemDataOfType(itemType);
            _itemGenerator.GeneratePool(itemData.ItemPrefab, itemData.ConfigureData.ItemPoolSize);
        }

        private void SetItemWithoutMatch(IGridSlot slot)
        {
            // GridItem item = colorType == 4 ? _itemGenerator.GetRedNormalItem() : _itemGenerator.GetRandomNormalItem();
            GridItem item = _itemGenerator.GetRandomNormalItem();
            item.SetBoard(_board);
            _itemGenerator.SetItemOnSlot(item, slot);
        }
        
    }
}