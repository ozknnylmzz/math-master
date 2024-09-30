using System.Collections.Generic;
using Math.Boards;

namespace Math.Matchs
{
    public class BoardDropItemData
    {
        public HashSet<ItemFallData> DropDatas { get; private set; }
        
        public bool MatchExists => DropDatas.Count != 0;

        public BoardDropItemData()
        {
            DropDatas = new HashSet<ItemFallData>();
        }

        public void SetMatchDatas(HashSet<ItemFallData> dropDatas)
        {
            DropDatas = dropDatas;
        }
        
        public void AddDropData(ItemFallData itemFallData)
        {
            if (DropDatas == null)
            {
                DropDatas = new HashSet<ItemFallData>();
            }

            DropDatas.Add(itemFallData);
        }
      
    }
}