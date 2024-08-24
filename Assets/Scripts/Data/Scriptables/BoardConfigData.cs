using Math.Boards;
using UnityEngine;

namespace Math.Data
{
    [CreateAssetMenu(menuName = "Board/BoardConfigData", order = 1)]
    public class BoardConfigData : ScriptableObject
    {
        public int RowCount;
        public int ColumnCount;
        public float CellSpacing;
        public GridSlot Grid;
    }
}
