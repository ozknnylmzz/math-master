using Cysharp.Threading.Tasks;
using DG.Tweening;
using Match3.Strategy;
using Math.Matchs;
using Math.Boards;
using Math.Items;
using Math.Strategy;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

namespace Math.Game
{
    public class GameController : MonoBehaviour
    {
        private IBoard _board;
        private GridPosition _selectedGridPosition;
        private IMatchDataProvider _matchDataProvider;
        private MatchClearStrategy _matchClearStrategy;
        public GridItem SelectedGridItem { get; private set; }
        private GridItem _targetGridItem;


        public void Initialize(StrategyConfig strategyConfig, IBoard board, GameConfig gameConfig)
        {
            _board = board;
            _matchDataProvider = gameConfig.MatchDataProvider;
            _matchClearStrategy = strategyConfig.MatchClearStrategy;
        }

        public bool IsPointerOnBoard(Vector3 pointerWorldPos, out GridPosition selectedGridPosition)
        {
            return _board.IsPointerOnBoard(pointerWorldPos, out selectedGridPosition);
        }

        public bool IsMatchDetected(out BoardDropItemData dropItemData, GridPosition selectedPositions,
            GridPosition targetPositions)
        {
            if (_board.GetNormalItem(selectedPositions).ColorType != _board.GetNormalItem(targetPositions).ColorType)
            {
                dropItemData = null;
                return false;
            }

            dropItemData = _matchDataProvider.GetMatchData(_board, selectedPositions, targetPositions);

            return dropItemData.MatchExists;
        }

        public async void SwapItemsAsync(GridPosition selectedPosition, GridPosition targetPosition)
        {
            IGridSlot selectedSlot = _board[selectedPosition];
            IGridSlot targetSlot = _board[targetPosition];
            await DoNormalSwap(selectedSlot, targetSlot);
        }

        private async UniTask DoNormalSwap(IGridSlot selectedSlot, IGridSlot targetSlot)
        {
            // // await SwapItemsAnimation(selectedSlot, targetSlot);
            //
            // if (IsMatchDetected(out BoardMatchData boardMatchData, selectedSlot.GridPosition, targetSlot.GridPosition))
            // {
            //     _matchClearStrategy.CalculateMatchStrategyJobs(boardMatchData);
            //
            //     // CheckAutoMatch();
            // }
            // else
            // {
            //     // SwapItemsBack(selectedSlot, targetSlot);
            // }
        }

        public bool CheckMove(Vector3 targetWorldPos)
        {
            _targetGridItem = _board.GetGridItem(targetWorldPos);

            if (_targetGridItem == null)
            {
                SelectedGridItem.SetWorldPosition(targetWorldPos);
                return true;
            }

            if (SelectedGridItem == null || _targetGridItem == null || SelectedGridItem == _targetGridItem)
            {
                return SelectedGridItem != null && SelectedGridItem == _targetGridItem;
            }

            if (IsMatchDetected(out BoardDropItemData boardDropItemData, SelectedGridItem.ItemSlot.GridPosition,
                    _targetGridItem.ItemSlot.GridPosition))
            {
                _matchClearStrategy.CalculateMatchStrategyJobs(boardDropItemData, SelectedGridItem, _targetGridItem);
                return true;
            }

            return false;
        }

        public void SetSelectedItem(GridPosition gridPosition)
        {
            SelectedGridItem = _board.GetNormalItem(gridPosition);
        }

        public void SetItemToMove(Vector2 pointerWorldPos)
        {
            IGridSlot gridSlot = _board.GetGridSlot(pointerWorldPos);
            if (!BoardHelper.IsItemBelow(gridSlot, _board, out IGridSlot targetSlot))
            {
                SelectedGridItem.SetItemPosition(gridSlot.GridPosition);
                gridSlot.ClearSlot();
                // targetSlot.ClearSlot();
                SelectedGridItem.DoMove(targetSlot).OnComplete((() => SetItemToMove(targetSlot.WorldPosition)));
            }
            else
            {
                gridSlot.SetItem(SelectedGridItem);
            }
        }

        // public bool IsPositionInBounds()
        // {
        //     _board.IsPointerOnBoard()
        // }
    }
}