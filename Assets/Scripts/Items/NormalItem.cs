using Math.Boards;
using Math.Data;
using Math.Enums;
using Math.InputSytem;
using UnityEngine;

namespace Math.Items
{
    public class NormalItem : SpriteItem
    {
        [SerializeField] private ColoredItemConfigureData _configureData;
        public override ItemType ItemType => ItemType.BoardItem;
        
        public override void ConfigureItem(int configureType)
        {
            SetConfigureType(configureType);
            SetContentData(_configureData.ColoredItemDatas[configureType]);
        }

        public override void Kill(bool shouldPlayExplosion = true, bool isSpecialKill = true)
        {
            base.Kill();
        }

        // public void SetPositionOnBoard(Vector2 inputPosition)
        // {
        //     Debug.Log("inputPosition"+inputPosition);
        //     Board.IsPointerOnBoard(inputPosition, out GridPosition gridPosition);
        //     Debug.Log("gridPosition"+gridPosition);
        //
        //     SetItemPosition(gridPosition);
        // }

        private void SetContentData(ColoredItemData itemContentData)
        {
            SetColorType(itemContentData.colorType);
            SetSprite(_configureData.Sprite);
            SetColor(itemContentData.ItemColor);
            SetText(itemContentData.Value.ToString());
            SetTextColor(itemContentData.TextColor);
            SetCanvas();
        }

    } 
}