
using Cysharp.Threading.Tasks;
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
        private GridItem _selectedGridItem;
        private GridItem _targetGridItem;

        public void Initialize(StrategyConfig strategyConfig,  IBoard board,GameConfig gameConfig)
        {
            _board = board;
             _matchDataProvider = gameConfig.MatchDataProvider;
             _matchClearStrategy = strategyConfig.MatchClearStrategy;
        }
        
        public bool IsPointerOnBoard(Vector3 pointerWorldPos, out GridPosition selectedGridPosition)
        {
            return _board.IsPointerOnBoard(pointerWorldPos, out selectedGridPosition);
        }
        
        public bool IsMatchDetected(out BoardMatchData boardMatchData, params GridPosition[] gridPositions)
        {
            boardMatchData = _matchDataProvider.GetMatchData(_board, gridPositions);

            return boardMatchData.MatchExists;
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

            if (_selectedGridItem == null || _targetGridItem == null || _selectedGridItem == _targetGridItem)
            {
                return _selectedGridItem != null && _selectedGridItem == _targetGridItem;
            }

            if (_selectedGridItem.ColorType == _targetGridItem.ColorType)
            {
                _matchClearStrategy.CalculateMatchStrategyJobs(_selectedGridItem, _targetGridItem);
                return true;
            }

            return false;
        }

        public void SetSelectedItem(Vector2 pointerWorldPos)
        {
            _selectedGridItem = _board.GetGridItem(pointerWorldPos);
        }
        
    }
}
