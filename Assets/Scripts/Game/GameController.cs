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
        public bool JobCompleted { get; private set; }


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

        public bool IsMatchDetected(GridPosition selectedPositions, GridPosition targetPositions)
        {
            if (_board.GetNormalItem(selectedPositions)?.ColorType != _board.GetNormalItem(targetPositions)?.ColorType)
            {
                return false;
            }

            return true;
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
        
        public void ItemSetPosition(Vector2 position)
        {
            SelectedGridItem.SetWorldPosition(position);
        }
        public bool CheckMove(Vector3 targetWorldPos)
        {
            _targetGridItem = _board.GetGridItem(targetWorldPos);

            if (_targetGridItem == null)
            {
                ItemSetPosition(targetWorldPos);
                return true;
            }

            if (SelectedGridItem == null || _targetGridItem == null || SelectedGridItem == _targetGridItem)
            {
                return SelectedGridItem != null && SelectedGridItem == _targetGridItem;
            }

            if (IsMatchDetected( SelectedGridItem.ItemSlot.GridPosition, _targetGridItem.ItemSlot.GridPosition))
            {
                _matchClearStrategy.CalculateMatchStrategyJobs(SelectedGridItem, _targetGridItem);
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
            // Tahtadaki hedef grid slotu alıyoruz
            IGridSlot gridSlot = _board.GetGridSlot(pointerWorldPos);

            // Eğer bu slotun altında bir item yoksa, seçili item'in slotunu temizliyoruz
            if (!BoardHelper.IsItemBelow(gridSlot, _board, out IGridSlot targetSlot))
            {
                SelectedGridItem.ItemSlot.ClearSlot();
            }

            // Grid slot'a seçili item'i yerleştiriyoruz
            gridSlot.SetItem(SelectedGridItem);
            SelectedGridItem.SetBoard(_board);
            SelectedGridItem.SetItemPosition(gridSlot.GridPosition);

            // Animasyon işlemleri için bir Sequence oluşturuyoruz
            Sequence sequence = DOTween.Sequence();

            // Eğer hedef slotun altında boş slotlar varsa, item düşüş animasyonu başlatıyoruz
            if (BoardHelper.GetAllEmptySlotsBelow(gridSlot, _board, out ItemFallData itemFallData))
            {
                // İlk slotu temizleyip, hedef slot'a item'i yerleştiriyoruz
                gridSlot.ClearSlot();
                SelectedGridItem.ItemSlot.ClearSlot();
                SelectedGridItem.SetItemPosition(gridSlot.GridPosition);
                targetSlot.SetItem(SelectedGridItem);

                // Animasyonla item'i hareket ettiriyoruz, ardından aynı fonksiyonu yeniden çağırıyoruz
                SelectedGridItem?.DoMove(targetSlot).OnComplete(() => SetItemToMove(targetSlot.WorldPosition));

                // Eğer boş slotlar mevcutsa, item'lerin düşmesini sağlıyoruz
                if (itemFallData != null)
                {
                    Debug.Log("itemFallData: " + itemFallData.Item);
                    GetFallTween(itemFallData);
                }
            }
            // Eğer hedef slot boşsa, item'i buraya yerleştiriyoruz
            else if (!gridSlot.HasItem)
            {
                SelectedGridItem.ItemSlot.ClearSlot();
                gridSlot.SetItem(SelectedGridItem);
            }
        }

        private Tween GetFallTween(ItemFallData itemFallData)
        {
            // SelectedGridItem.SetItemPosition(itemFallData.Item.ItemSlot.GridPosition);
            itemFallData.Item.ItemSlot.ClearSlot();
            itemFallData.DestinationSlot.SetItem(SelectedGridItem);

            Transform item = itemFallData.Item.transform;
            float pathDistance = itemFallData.PathDistance;

            float fallDuration = pathDistance <= 4
                ? 0.08f * pathDistance + 0.08f
                : pathDistance * 0.1f;

            return item.DOMove(itemFallData.DestinationSlot.WorldPosition, fallDuration)
                .SetEase(Ease.InSine)
                .OnComplete(() =>
                {
                    BoardHelper.IsItemBelow(itemFallData.DestinationSlot, _board, out IGridSlot targetSlot);
                    if (IsMatchDetected(SelectedGridItem.ItemSlot.GridPosition,targetSlot.GridPosition ))
                    {
                        Debug.Log("SelectedGridItem"+SelectedGridItem.ItemSlot.GridPosition);
                        _matchClearStrategy.CalculateMatchStrategyJobs(itemFallData.DestinationSlot.Item, targetSlot.Item);
                    }
                });
        }
        
        
        public bool IsPositionInLimited(Vector2 position, out Vector2 limitedPosition)
        {
            limitedPosition = position;
            bool isOutOfBounds = false;

            // Y ekseninde sınır kontrolü
            if (position.y > _board.GetTopSlotY())
            {
                limitedPosition.y = _board.GetTopSlotY();
                isOutOfBounds = true;
            }
            else if (position.y < _board.GetBottomSlotY())
            {
                limitedPosition.y = _board.GetBottomSlotY();
                isOutOfBounds = true;
            }
            else
            {
                // Eğer Y ekseni sınır içindeyse, Y pozisyonunu orijinalde tut
                limitedPosition.y = position.y;
            }

            // X ekseninde sınır kontrolü
            if (position.x < _board.GetLeftSlotX())
            {
                limitedPosition.x = _board.GetLeftSlotX();
                isOutOfBounds = true;
            }
            else if (position.x > _board.GetRightSlotX())
            {
                limitedPosition.x = _board.GetRightSlotX();
                isOutOfBounds = true;
            }
            else
            {
                // Eğer X ekseni sınır içindeyse, X pozisyonunu orijinalde tut
                limitedPosition.x = position.x;
            }

            // Eğer pozisyon sınırların dışındaysa, limitedPosition güncellenmiş olacak
            return isOutOfBounds;
        }


    }
}