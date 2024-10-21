using System.Collections;
using System.Collections.Generic;
using Math.Data;
using Math.Items;
using UnityEngine;

namespace Math.Boards
{
    public class Board : MonoBehaviour, IBoard
    {
        [SerializeField] private BoardConfigData _boardConfigData;

        private const float _cellSize = 1f;
        private Vector3 _originPos;

        private IGridSlot[,] _gridSlots;
        private GridPosition[] _allGridPositions;

        public IGridSlot this[GridPosition gridPosition] => _gridSlots[gridPosition.RowIndex, gridPosition.ColumnIndex];

        public IGridSlot this[int rowIndex, int columnIndex] => _gridSlots[rowIndex, columnIndex];

        public GridPosition[] AllGridPositions => _allGridPositions;

        public int RowCount => _boardConfigData.RowCount;
        public int ColumnCount => _boardConfigData.ColumnCount;

        // X ekseni sınırları (sol ve sağ sınır)
        public float LeftBoundX { get; private set; }
        public float RightBoundX { get; private set; }

        public void Initialize()
        {
            GenerateGrid();

            // Sınırları belirliyoruz (X ekseni için)
            LeftBoundX = _originPos.x - (_cellSize / 2); // Sol sınır
            RightBoundX = _originPos.x + (ColumnCount * (_cellSize + _boardConfigData.CellSpacing)) -
                          (_cellSize / 2); // Sağ sınır
        }

        private void GenerateGrid()
        {
            _gridSlots = new IGridSlot[RowCount, ColumnCount];
            _allGridPositions = new GridPosition[RowCount * ColumnCount];

            _originPos = GetOriginPosition(RowCount, ColumnCount);

            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    int iteration = i * ColumnCount + j;
                    Vector3 slotPosition = GridToWorldPosition(i, j);

                    GridSlot gridSlot =
                        Instantiate(_boardConfigData.Grid, slotPosition, Quaternion.identity, transform);
                    gridSlot.name = "(" + i + " , " + j + ")";

                    GridPosition gridPosition = new GridPosition(i, j);
                    gridSlot.SetPosition(gridPosition, slotPosition);

                    _gridSlots[i, j] = gridSlot;
                    _allGridPositions[iteration] = gridPosition;
                }
            }
        }

        public bool IsPointerOnBoard(Vector3 pointerWorldPos, out GridPosition gridPosition)
        {
            gridPosition = WorldToGridPosition(pointerWorldPos);
            
            return IsPositionOnBoard(gridPosition);
            gridPosition = WorldToGridPosition(pointerWorldPos);
            GridItem item = GetNormalItem(gridPosition);
            item?.SetWorldPosition(pointerWorldPos);
            return IsPositionOnBoard(gridPosition);
        }


        public bool IsPositionInBounds(GridPosition gridPosition)
        {
            return gridPosition.IsPositionInBounds(RowCount, ColumnCount);
        }

        private bool IsPositionOnBoard(GridPosition gridPosition)
        {
            return IsPositionInBounds(gridPosition);
        }

        public bool IsPositionOnItem(GridPosition gridPosition)
        {
            return IsPositionInBounds(gridPosition) && this[gridPosition].CanContainItem;
        }

        public GridItem GetGridItem(Vector3 pointerWorldPos)
        {
            GridPosition gridPosition = WorldToGridPosition(pointerWorldPos);

            return GetNormalItem(gridPosition);
        }

        public IGridSlot GetGridSlot(Vector3 pointerWorldPos)
        {
            GridPosition gridPosition = WorldToGridPosition(pointerWorldPos);
            Debug.Log("GridPosition"+gridPosition.RowIndex+"----"+ gridPosition.ColumnIndex);
            return GetSlot(gridPosition.RowIndex, gridPosition.ColumnIndex);
        }

        private GridPosition WorldToGridPosition(Vector3 pointerWorldPos)
        {
            Vector2 gridPos = (pointerWorldPos - _originPos) / (_cellSize + _boardConfigData.CellSpacing);

            int rowIndex = Mathf.FloorToInt(gridPos.y + _cellSize / 2);
            int columnIndex = Mathf.FloorToInt(gridPos.x + _cellSize / 2);

            return new GridPosition(rowIndex, columnIndex);
        }

        public Vector3 GridToWorldPosition(GridPosition gridPosition)
        {
            return GridToWorldPosition(gridPosition.RowIndex, gridPosition.ColumnIndex);
        }

        public float GetTopSlotY()
        {
            return GridToWorldPosition(RowCount, ColumnCount).y - _cellSize ;
        }
        
        public float GetBottomSlotY()
        {
            return GridToWorldPosition(0 , 0).y + _cellSize;
        }

        public float GetLeftSlotX()
        {
            return GridToWorldPosition(0, 0).x; // İlk slotun X pozisyonu
        }

        public float GetRightSlotX()
        {
            return GridToWorldPosition(0, ColumnCount - 1).x; // Son slotun X pozisyonu
        }
        
        private Vector3 GridToWorldPosition(int rowIndex, int columnIndex)
        {
            return new Vector3(columnIndex, rowIndex) * (_cellSize + _boardConfigData.CellSpacing) + _originPos;
        }

        private Vector3 GetOriginPosition(int rowCount, int columnCount)
        {
            float gridWidth = columnCount * _cellSize + (columnCount - 1) * _boardConfigData.CellSpacing;
            float gridHeight = rowCount * _cellSize + (rowCount - 1) * _boardConfigData.CellSpacing;

            float originX = -gridWidth / 2.0f + _cellSize / 2;
            float originY = -gridHeight / 2.0f + _cellSize / 2;

            return new Vector3(originX, originY);
        }

        public GridItem GetNormalItem(GridPosition gridPosition)
        {
            if (!IsPositionInBounds(gridPosition))
            {
                Debug.Log("Grid pozisyonu tahtanın sınırlarında değil: " + gridPosition);
                return null;
            }

            IGridSlot gridSlot = this[gridPosition];

            // Grid slot null veya boş olabilir, kontrol edelim
            if (gridSlot == null || gridSlot.Item == null)
            {
                Debug.Log("Bu pozisyonda grid slot veya item yok: " + gridPosition);
                return null;
            }

            // Eğer geçerli bir item varsa, bunu döndürüyoruz
            return gridSlot.Item;
        }

        public IGridSlot GetSlot(int rowIndex, int columnIndex)
        {
            return this[rowIndex, columnIndex];
        }

        public IEnumerator<IGridSlot> GetEnumerator()
        {
            foreach (IGridSlot slot in _gridSlots)
            {
                yield return slot;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

