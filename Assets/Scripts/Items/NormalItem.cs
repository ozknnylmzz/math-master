using Math.Data;
using Math.Enums;
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

        private void SetContentData(ColoredItemData itemContentData)
        {
            SetColorType(itemContentData.colorType);
            SetSprite(itemContentData.Sprite);
        }

    } 
}