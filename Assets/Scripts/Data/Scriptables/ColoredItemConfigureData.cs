using Math.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace Math.Data
{
    [CreateAssetMenu(menuName = "Board/ConfigureData/ColoredItemConfigureData")]
    public class ColoredItemConfigureData : ConfigureData
    { 
        public override ContentData[] ContentDatas => ColoredItemDatas;

        public ColoredItemData[] ColoredItemDatas;
        public Sprite Sprite;
    }

    [System.Serializable]
    public class ColoredItemData : ContentData
    {
        public ColorType colorType;
        
        [FormerlySerializedAs("Color")] public Color ItemColor;
        public Color TextColor;
        public int Value;
    }
}